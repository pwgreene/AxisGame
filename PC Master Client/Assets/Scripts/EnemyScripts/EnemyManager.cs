using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{

    public GameObject enemy;

	public GameObject enemyWarning;
	PhotonView pv;

	int numRemainingEnemies; //number of enemies alive on the map
	int totalEnemies;		 //number of enemies that will spawn total
	int numEnemiesToSpawn;   //number of enemies left to spawn

    public float spawnTime;

    // Use this for initialization
    void Start()
    {	
		pv = PhotonView.Get(this);
		if (PhotonNetwork.isMasterClient) {
			StartCoroutine(SpawnEnemies());
		}

        
    }
	void Update () {
		if(!pv.isMine){
			//nothing rightn ow
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		//nothing right now, need this to be implemented if script is being observed
	}
	public void setNumEnemiesToSpawn(int num) {
		numEnemiesToSpawn = num;
		numRemainingEnemies = numEnemiesToSpawn;
		totalEnemies = numEnemiesToSpawn;
	}

	//used by child to say that he is dead
	public void killEnemy() {
		numRemainingEnemies--;
		if (numRemainingEnemies == 0) {
			//let the wave manager know all of your enemies are killed
			if (PhotonNetwork.isMasterClient) {
				WaveManager waveManager = GameObject.FindGameObjectWithTag("WaveManager").GetComponent<WaveManager> ();
				waveManager.EnemyManagerDone(totalEnemies);
			}
			PhotonNetwork.Destroy (gameObject);
		}
	}

    IEnumerator SpawnEnemies()
    {
		//wait for enemy type
		if (enemy == null) {
			yield return new WaitForSeconds (1);
		}

        while (numEnemiesToSpawn > 0)
        {
            // Choose a random position off screen
            float fixedValue = Random.value > 0.5f ? -0.2f : 1.2f;
            float randomValue = Random.value;

            Vector3 enemyScreenPoint;
			Vector3 warningScreenPoint;

            if (Random.value > 0.5f)
            {
                enemyScreenPoint = new Vector3(fixedValue, randomValue, 10);
				if (fixedValue >= 0) { //spawing point is on right of screen
					warningScreenPoint = new Vector3 (.9f, .5f, 10);
				} else {
					warningScreenPoint = new Vector3 (.1f, .5f, 10);
				}
            }
            else
            {
                enemyScreenPoint = new Vector3(randomValue, fixedValue, 10);
				if (fixedValue >= 0) { //spawing point is on top of screen
					warningScreenPoint = new Vector3 (.5f, .9f, 10);
				} else {
					warningScreenPoint = new Vector3 (.5f, .1f, 10);
				}
            }
            
            Vector3 enemyWorldPoint = Camera.main.ViewportToWorldPoint(enemyScreenPoint);
			Vector3 warningWorldPoint = Camera.main.ViewportToWorldPoint (warningScreenPoint);
			pv.RPC("InstantiateWarning", PhotonTargets.All, warningWorldPoint);
            //GameObject newEnemy = (GameObject)Instantiate(enemy, enemyWorldPoint, Quaternion.identity);
			if (numEnemiesToSpawn >= 10) {
				spawnCluster (5, 1, enemyWorldPoint, enemy);
				numEnemiesToSpawn -= 5;
			} else {
				GameObject newEnemy = PhotonNetwork.InstantiateSceneObject (enemy.name, enemyWorldPoint, Quaternion.identity, 0, null);
				//newEnemy.transform.parent = transform;
				print ("enemy spawned");
				numEnemiesToSpawn -= 1;
			}
            yield return new WaitForSeconds(spawnTime);
        }


    }

	[PunRPC] 
	void InstantiateWarning(Vector3 point){
		Instantiate (enemyWarning, point, Quaternion.identity);
	}

	void spawnCluster(int enemyCount, int radius, Vector3 location, GameObject enemyType) {
		Vector3 gap = new Vector3 (radius, 0, 0);
		for (int i = 0; i < enemyCount; i++) {
			GameObject enemy = PhotonNetwork.InstantiateSceneObject (enemyType.name, location + gap, Quaternion.identity, 0, null);
			EnemyBehaviour behaviour = enemy.GetComponent<EnemyBehaviour> ();

			if (behaviour != null) {
				//behaviour.speed = 0;
				gap = Quaternion.Euler (0, 0, 360.0f / enemyCount) * gap;
				continue;
			}
		}
	}
}
