using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{

    public GameObject enemy;
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

            Vector3 screenPoint;

            if (Random.value > 0.5f)
            {
                screenPoint = new Vector3(fixedValue, randomValue, 10);
            }
            else
            {
                screenPoint = new Vector3(randomValue, fixedValue, 10);
            }
            
            Vector3 worldPoint = Camera.main.ViewportToWorldPoint(screenPoint);
            GameObject newEnemy = (GameObject)Instantiate(enemy, worldPoint, Quaternion.identity);
            newEnemy.transform.parent = transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
