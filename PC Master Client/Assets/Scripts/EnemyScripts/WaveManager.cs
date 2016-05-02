using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour {

	int waveNumber;
	int numEnemiesOnWave;

	public GameObject enemyManager;
	public GameObject smallSuicider;
	public GameObject mediumSuicider;
	public GameObject largeSuicider;
	public GameObject enemyBoss;
	PhotonView pv;
    public GUIManager guiM;

	// Use this for initialization
	void Start () {
		waveNumber = 1;
		SpawnWave ();
        guiM = GUIManager.Instance;
		pv = PhotonView.Get (this);
	}
	
	// Update is called once per frame
	void Update () {
		//nothing right now
		if(!pv.isMine){
			
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//nothing right now
	}

	void InitializeManager(GameObject manager, GameObject enemyType, int numEnemies, float spawnTime) {
		if (PhotonNetwork.isMasterClient) {
			EnemyManager enemyManager = manager.GetComponent<EnemyManager> ();
			enemyManager.enemy = enemyType;
			enemyManager.setNumEnemiesToSpawn (numEnemies);
			enemyManager.spawnTime = spawnTime;
		}

		//manager.transform.parent = transform;
	}


	public void EnemyManagerDone(int numEnemies) {
		numEnemiesOnWave -= numEnemies;
		print ("manager done"+numEnemiesOnWave.ToString());
		//all managers should be done, wave over
		if (numEnemiesOnWave <= 0) {
			waveNumber++;

			pv.RPC ("ChangeWaveNumber", PhotonTargets.AllBufferedViaServer, waveNumber);
            SpawnWave ();
		}
	}

	[PunRPC]
	void ChangeWaveNumber(int waveNum){
		waveNumber = waveNum;
		guiM.UpdateWaveNumber(waveNumber);
	}
	void SpawnWave() {
		if (PhotonNetwork.isMasterClient && PhotonNetwork.playerList.Length > 0) {
			GameObject manager = PhotonNetwork.InstantiateSceneObject ("EnemyManager", transform.position, transform.rotation,0,null);
			numEnemiesOnWave = waveNumber*2 + 5;
			int numTypesOfWaves = 3;
			float spawnFrequency;
			switch ((waveNumber-1) % numTypesOfWaves) {
			case 0:
				spawnFrequency = 3f;
				//InitializeManager (manager, enemyBoss, 1, spawnFrequency);
				InitializeManager (manager, smallSuicider, numEnemiesOnWave, spawnFrequency);
				break;
			case 1:
				spawnFrequency = 3f;
				InitializeManager (manager, mediumSuicider, numEnemiesOnWave, spawnFrequency);
				break;
			case 2:
				spawnFrequency = 3f;
				InitializeManager (manager, largeSuicider, numEnemiesOnWave, spawnFrequency);
				break;
			case 3:
				spawnFrequency = 3f;
				InitializeManager (manager, enemyBoss, (int)waveNumber / numEnemiesOnWave + 1, spawnFrequency);
				break;
			}
		}

	}
		
	void OnGUI() {
		GUI.Box (new Rect (50, 300, 100, 50), "Wave:  " + waveNumber.ToString());
	}
		
}
