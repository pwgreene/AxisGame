using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{

    public GameObject enemy;
	public GameObject enemyWarning;
    public float spawnTime;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
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
			Instantiate (enemyWarning, warningWorldPoint, Quaternion.identity);
            //GameObject newEnemy = (GameObject)Instantiate(enemy, enemyWorldPoint, Quaternion.identity);
			GameObject newEnemy = PhotonNetwork.InstantiateSceneObject(enemy.name, enemyWorldPoint, Quaternion.identity, 0,null);
            //newEnemy.transform.parent = transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
