using UnityEngine;
using System.Collections;
using System;

public class WarningBehavior : MonoBehaviour {

	public int lifespan;
	int life;

	public Vector3 corePosition;

	// Use this for initialization
	void Start () {
		life = 0;
		try
		{
			corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
		}
		catch (NullReferenceException)
		{
			Destroy(gameObject);
		}
		//point towards core and move onto screen
		Vector3 direction = corePosition - transform.position;
		float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
		life++;
		if (life >= lifespan) {
			Destroy (gameObject);
		}
	}
}
