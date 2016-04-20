using UnityEngine;
using System.Collections;

public class MissileLauncher : MonoBehaviour, Weapon {

	public GameObject projectile;
	public int fireRate;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void Fire(){
			Instantiate (projectile, this.gameObject.transform.position, this.gameObject.transform.rotation);
	}

	public int getFireRate(){
		return fireRate;
	}


}
