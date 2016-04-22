using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	int waveNumber;
	int numEnemiesOnWave;

	public int enemiesOnFirstWave;

	public GameObject enemyManager;
	public GameObject smallSuicider;
	public GameObject mediumSuicider;
	public GameObject largerSuicider;

	// Use this for initialization
	void Start () {
		numEnemiesOnWave = enemiesOnFirstWave;
		GameObject firstManager = (GameObject) Instantiate (enemyManager, transform.position, transform.rotation);
		InitializeManager (firstManager, smallSuicider, numEnemiesOnWave);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitializeManager(GameObject manager, GameObject enemyType, int numEnemies) {
		EnemyManager enemyManager = manager.GetComponent<EnemyManager> ();
		enemyManager.enemy = enemyType;
		enemyManager.setNumEnemiesToSpawn (numEnemies);
		manager.transform.parent = transform;
	}

	public void EnemyManagerDone(int numEnemies) {
		numEnemiesOnWave -= numEnemies;
		print ("wave done");
	}
}
