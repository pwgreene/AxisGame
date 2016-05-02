using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {
	public float minTimeBetweenPowerUps;
	public float maxTimeBetweenPowerUps;
	//public GameObject powerupPrefab;
	List<Vector2> powerUpSpawnPoints;

	public float ammo;
	public float heal_amount;
	public float fireRate_decrease_factor;
	public float fireRate_duration;

	public Sprite grenade_icon;
	public Sprite missile_icon;
	public Sprite heal_icon;
	public Sprite fire_rate_icon;

	// Use this for initialization
	void Start () {
		powerUpSpawnPoints = new List<Vector2> ();
		powerUpSpawnPoints.Add (new Vector2 (-5f, -3f));
		powerUpSpawnPoints.Add (new Vector2 (3f, -4f));
		powerUpSpawnPoints.Add (new Vector2 (7f, 2.5f));
		powerUpSpawnPoints.Add (new Vector2 (-3f, 4f));
		StartCoroutine ("spawnPowerUp");
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//nothing right now, need this to be implemented if script is being observed
	}

	IEnumerator spawnPowerUp(){
		if (PhotonNetwork.isMasterClient) {


			int powType = (int)Mathf.RoundToInt (Random.Range (0, System.Enum.GetValues (typeof(PowerupType)).Length));
			//GameObject clone = Instantiate (powerupPrefab, powerUpSpawnPoints [Mathf.RoundToInt(Random.Range (0, powerUpSpawnPoints.Count - 1))], Quaternion.identity) as GameObject;
			GameObject clone = PhotonNetwork.InstantiateSceneObject ("Powerup", powerUpSpawnPoints [Mathf.RoundToInt (Random.Range (0, powerUpSpawnPoints.Count - 1))], Quaternion.identity, 0, null);
			Powerups scriptPower = clone.GetComponent<Powerups> ();
			PhotonView pvClone = clone.GetComponent<PhotonView> ();
			pvClone.RPC ("SetPowType", PhotonTargets.AllBuffered, powType);
			SpriteRenderer icon = clone.transform.FindChild ("icon").GetComponent<SpriteRenderer> ();
			switch (scriptPower.powType) {

			case PowerupType.AmmoIncrease_Grenade:
				scriptPower.ammoCount = ammo;
				icon.sprite = grenade_icon;
				break;

			case PowerupType.AmmoIncrease_Missile:
				scriptPower.ammoCount = ammo;
				icon.sprite = missile_icon;
				icon.transform.localScale = new Vector3 (1, 1, 1);
				break;

			case PowerupType.CoreHealth:
				scriptPower.healAmount = heal_amount;
				icon.sprite = heal_icon;
				break;

			case PowerupType.FiringRate:
				scriptPower.fireRateIncrease = fireRate_decrease_factor;
				scriptPower.rateIncreaseDuration = fireRate_duration;
				icon.sprite = fire_rate_icon;
				break;
			}

			yield return new WaitForSeconds (Random.Range (minTimeBetweenPowerUps, maxTimeBetweenPowerUps));
			StartCoroutine ("spawnPowerUp");
		}
	}


}
//Previous values: Speed, CoreHealth, FiringRate, CoreRotationSpeed
public enum PowerupType{CoreHealth, FiringRate, AmmoIncrease_Missile, AmmoIncrease_Grenade}