using UnityEngine;
using System.Collections;
using System;

public class EnemyBossBehaviour : EnemyBehaviour {

	public GameObject shield;
	public int shieldCooldown;
	public int shieldLife;

	bool hasShield;
	int timeToSpawnShield;
	int timeToDestroyShield;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		float distance = (corePosition - transform.position).magnitude;
		//move a fixed distance away from the core
		if (distance > 10f) {
			moveTowardCore ();
		} else if (!hasShield) { //stopped moving, spawn shield
			pv.RPC("InstantiateShield",PhotonTargets.AllBufferedViaServer);
		}
	}

	[PunRPC]
	void InstantiateShield(){
		GameObject newShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
		newShield.transform.parent = transform;
		hasShield = true;
	}

}
