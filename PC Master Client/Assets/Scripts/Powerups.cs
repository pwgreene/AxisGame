using UnityEngine;
using System.Collections;

public class Powerups : MonoBehaviour {
	
	public PowerupType powType;

	public float ammoCount;

	public float healAmount;

	public float fireIntervalInverseFactor;
	public float rateIncreaseDuration;
	public SpriteRenderer icon;
	private GameObject core;
	// Use this for initialization
	void Start () {

		//currently none of these effects are temporary

		//should be with find tag, but right now player is child of the core so they're all tagged core
		core = GameObject.FindGameObjectWithTag("Core");
		if (core == null) {
			print ("Why doesn't this work?!");
		}
	}

	[PunRPC]
	void SetPowType(int type){
		powType = (PowerupType)type;
		//SpriteRenderer icon = GetComponentInChildren<SpriteRenderer> ();
		PowerupManager powMan = GameObject.FindGameObjectWithTag ("PowerupManager").GetComponent<PowerupManager> ();
		switch (powType) {

		case PowerupType.AmmoIncrease_Grenade:
			ammoCount = powMan.ammo;
			icon.sprite = powMan.grenade_icon;
			break;

		case PowerupType.AmmoIncrease_Missile:
			ammoCount = powMan.ammo;
			icon.sprite = powMan.missile_icon;
			icon.transform.localScale = new Vector3 (1, 1, 1);
			break;

		case PowerupType.CoreHealth:
			healAmount = powMan.heal_amount;
			icon.sprite = powMan.heal_icon;
			break;

		case PowerupType.FiringRate:
			fireIntervalInverseFactor = powMan.fireRate_decrease_factor;
			rateIncreaseDuration = powMan.fireRate_duration;
			icon.sprite = powMan.fire_rate_icon;
			break;
		}
	}

	[PunRPC]
	void DestroyPowerUp(){
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.Destroy(transform.gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Player") || other.tag.Equals("Core")) {
			//Debug.Log ("collided with powerup");
			TurretController turret = other.GetComponent<TurretController>();
			PhotonView pv = turret.GetComponent<PhotonView> ();
			if (pv.isMine) {
				switch (powType)
				{
				case PowerupType.CoreHealth:
					//need to make sure this is networked - so damage calls rpc
					core.GetComponent<RotatingCoreBehaviour> ().Damage ((int)(-1 * healAmount));
					break;
				case PowerupType.FiringRate:
					//The way this is organized right now, multiple fire rate powerups only increase the duration of the increase
					if (!turret.increased_fire_rate) {
						//rpc
						//PowerUpApply(bool inc_rate, int fire_rate, float dur,int ammo,float ammo_increase){
						int rate = Mathf.RoundToInt (other.GetComponent<TurretController> ().fireIntervalSeconds / fireIntervalInverseFactor) + 1;
						pv.RPC("PowerUpApply", PhotonTargets.AllBuffered, true, rate, rateIncreaseDuration ,0,0f);


					} else {
						pv.RPC("PowerUpApply", PhotonTargets.AllBuffered, false, 0, rateIncreaseDuration ,0,0f);
					}


					break;
				case PowerupType.AmmoIncrease_Grenade:
					pv.RPC("PowerUpApply", PhotonTargets.AllBuffered, false, 0, 0f ,2,ammoCount);

					break;
				case PowerupType.AmmoIncrease_Missile:
					pv.RPC("PowerUpApply", PhotonTargets.AllBuffered, false, 0, 0f ,1,ammoCount);

					break;
				default:
					break;
				}

				//destroy this
				PhotonView.Get(this).RPC("DestroyPowerUp",PhotonTargets.AllBuffered);


			}



		}

	}
}

