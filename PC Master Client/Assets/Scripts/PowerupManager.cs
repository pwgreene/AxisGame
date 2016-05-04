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



	IEnumerator spawnPowerUp(){
		print ("in coroutine spawn");
		if (PhotonNetwork.isMasterClient) {

			print ("spawning powerup");
			int powType = (int)Mathf.RoundToInt (Random.Range (0, System.Enum.GetValues (typeof(PowerupType)).Length));
			//GameObject clone = Instantiate (powerupPrefab, powerUpSpawnPoints [Mathf.RoundToInt(Random.Range (0, powerUpSpawnPoints.Count - 1))], Quaternion.identity) as GameObject;
			GameObject clone = PhotonNetwork.InstantiateSceneObject ("Powerup", powerUpSpawnPoints [Mathf.RoundToInt (Random.Range (0, powerUpSpawnPoints.Count - 1))], Quaternion.identity, 0, null);

			PhotonView pvClone = clone.GetComponent<PhotonView> ();
			pvClone.RPC ("SetPowType", PhotonTargets.AllBuffered, powType);




		}
		yield return new WaitForSeconds (Random.Range (minTimeBetweenPowerUps, maxTimeBetweenPowerUps));
		StartCoroutine ("spawnPowerUp");
	}


}
//Previous values: Speed, CoreHealth, FiringRate, CoreRotationSpeed
public enum PowerupType{CoreHealth, FiringRate, AmmoIncrease_Missile, AmmoIncrease_Grenade}