using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Cluster : MonoBehaviour {

	//public float speed;

	Vector3 corePosition;

	public GameObject enemyType;
	public Vector3 spawnPosition;
	public int enemyCount;

	public int radius;

	public List<GameObject> enemies;

	// Use this for initialization
	void Start () {
		try
		{
			corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
		}
		catch (NullReferenceException)
		{
			Destroy(gameObject);
		}

		spawn ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void spawn(){
		Vector3 gap = new Vector3 (radius, 0, 0);
		for (int i = 0; i < enemyCount; i++) {
			
			GameObject a = Instantiate(enemyType, this.gameObject.transform.position + gap, this.gameObject.transform.rotation) as GameObject;
			EnemyBehaviour behaviour = a.GetComponent<EnemyBehaviour> ();

			if (behaviour != null) {
				//behaviour.speed = 0;
				gap = Quaternion.Euler (0, 0, 360.0f / enemyCount) * gap;
				continue;
			}

			Enemy2Behavior behaviour2 = a.GetComponent<Enemy2Behavior> ();

			if (behaviour2 != null) {
				//behaviour2.speed = 0;
				gap = Quaternion.Euler (0, 0, 360.0f / enemyCount) * gap;
				continue;
			}

			Enemy3Behavior behaviour3 = a.GetComponent<Enemy3Behavior> ();

			if (behaviour3 != null) {
				//behaviour3.speed = 0;
				gap = Quaternion.Euler (0, 0, 360.0f / enemyCount) * gap;
				continue;
			}

			gap = Quaternion.Euler (0, 0, 360.0f / enemyCount) * gap;

		}
	}

}
