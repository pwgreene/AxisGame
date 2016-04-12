using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerupManager : MonoBehaviour {
	public float timeBetweenPowerUps;
	public GameObject powerupPrefab;
	List<Vector2> powerUpSpawnPoints;

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
		GameObject clone = Instantiate (powerupPrefab, powerUpSpawnPoints [Mathf.RoundToInt(Random.Range (0, powerUpSpawnPoints.Count - 1))], Quaternion.identity) as GameObject;
		Powerups scriptPower = clone.GetComponent<Powerups> ();
		scriptPower.powType = (PowerupType) Mathf.RoundToInt (Random.Range(0,System.Enum.GetValues(typeof(PowerupType)).Length-1)) ;
		scriptPower.scalarIncrease += Random.Range(0,1);
		scriptPower.percentageIncrease = Random.Range (1, 1.1f);
		yield return new WaitForSeconds(timeBetweenPowerUps);
		StartCoroutine ("spawnPowerUp");

	}

}
public enum PowerupType{Speed, CoreHealth, FiringRate, CoreRotationSpeed}