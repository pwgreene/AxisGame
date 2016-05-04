using UnityEngine;
using System.Collections;
using System;

public class EnemyBossBehaviour : EnemyBehaviour {

	public GameObject shield;
	public GameObject laser;

	private ShieldBehaviour newShieldBehaviour;
	public int shieldCooldown;
	public int shieldLife;
	public int fireRate;

	bool hasShield;
	int timeElapsedSinceFire;

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
		if (distance > 11f) {
			moveTowardCore ();
		} else if (!hasShield) { //stopped moving, spawn shield
			//needs to go here otherwise client keeps trying to spawn shield until it gets the message from server
			hasShield = true;
			pv.RPC("InstantiateShield",PhotonTargets.AllBuffered);
		}
		if (timeElapsedSinceFire < fireRate) {
			timeElapsedSinceFire++;
		} else if (timeElapsedSinceFire >= fireRate && distance <= 11f && PhotonNetwork.isMasterClient) {
			pv.RPC("FireLaser",PhotonTargets.All);
			timeElapsedSinceFire = 0;
		}
	}

	[PunRPC]
	void SetShieldActive(bool active) {
		if (null != newShieldBehaviour) {

			newShieldBehaviour.sprite.enabled = active;

			newShieldBehaviour.isActive = active;
			newShieldBehaviour.timer = 0;
		}
	}
	[PunRPC]
	void InstantiateShield(){
		if (PhotonNetwork.isMasterClient) {
			//GameObject newShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
			GameObject newShield = Instantiate(shield, transform.position, transform.rotation) as GameObject;
			newShieldBehaviour = newShield.GetComponent<ShieldBehaviour> ();
			newShield.transform.parent = transform;
			hasShield = true;
		}
	}
	[PunRPC]
	void FireLaser() {
		Instantiate (laser, transform.position, transform.rotation);
	}

}
