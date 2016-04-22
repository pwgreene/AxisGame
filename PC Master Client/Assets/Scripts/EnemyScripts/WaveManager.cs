using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	int waveNumber;
	int numEnemiesOnWave;

	public GameObject enemyManager;
	public GameObject smallSuicider;
	public GameObject mediumSuicider;
	public GameObject largerSuicider;

	// Use this for initialization
	void Start () {
		waveNumber = 1;
		SpawnWave ();
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
		print ("manager done"+numEnemiesOnWave.ToString());
		//all managers should be done, wave over
		if (numEnemiesOnWave <= 0) {
			waveNumber++;
			SpawnWave ();
		}
	}

	void SpawnWave() {
		GameObject manager = (GameObject)Instantiate (enemyManager, transform.position, transform.rotation);
		numEnemiesOnWave = waveNumber*2 + 1;
		switch ((waveNumber-1) % 3) {
		case 0:
			InitializeManager (manager, smallSuicider, numEnemiesOnWave);
			break;
		case 1:
			InitializeManager (manager, mediumSuicider, numEnemiesOnWave);
			break;
		case 2:
			InitializeManager (manager, smallSuicider, numEnemiesOnWave);
			break;
		}
	}
		
	void OnGUI() {
		GUI.Box (new Rect (50, 300, 100, 50), "Wave:  " + waveNumber.ToString());
	}
		
}
