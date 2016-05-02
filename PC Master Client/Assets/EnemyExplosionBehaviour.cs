using UnityEngine;
using System.Collections;

public class EnemyExplosionBehaviour : MonoBehaviour {

	public int lifetime;

	// Use this for initialization
	void Start () {
		Invoke ("SelfDestruct", lifetime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SelfDestruct() {
		Destroy (gameObject);
	}
}
