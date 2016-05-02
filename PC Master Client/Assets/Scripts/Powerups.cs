using UnityEngine;
using System.Collections;

public class Powerups : MonoBehaviour {
	
	public PowerupType powType;

	public float ammoCount;

	public float healAmount;

	public float fireRateIncrease;
	public float rateIncreaseDuration;

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
			switch (powType)
			{
			case PowerupType.CoreHealth:
				core.GetComponent<RotatingCoreBehaviour> ().currentHP += healAmount; 
				break;
			case PowerupType.FiringRate:
				//The way this is organized right now, multiple fire rate powerups only increase the duration of the increase
				if (!turret.increased_fire_rate) {
					turret.increased_fire_rate = true;
					turret.fireRate = Mathf.RoundToInt (other.GetComponent<TurretController> ().fireRate / fireRateIncrease) + 1;
					turret.increased_fire_rate_duration += rateIncreaseDuration;
				} else {
					turret.increased_fire_rate_duration += rateIncreaseDuration;
				}


				break;
			case PowerupType.AmmoIncrease_Grenade:
				turret.ammoAmmounts [2] += ammoCount;
				break;
			case PowerupType.AmmoIncrease_Missile:
				turret.ammoAmmounts [1] += ammoCount;
				break;
			default:
				break;
			}


			//destroy this
			PhotonView.Get(this).RPC("DestroyPowerUp",PhotonTargets.MasterClient);

		}

	}
}

