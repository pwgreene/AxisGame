﻿using UnityEngine;
using System.Collections;
using System;

public class EnemyLaserBehaviour : MonoBehaviour {

	public int damage;
	public int speed;

	Vector3 corePosition;

	public SpriteRenderer sprite;
	public Rigidbody2D rb;
	public PhotonView pv;

	void Start()
	{
		try
		{
			corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
		}
		catch (NullReferenceException)
		{

			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("DestroyLaser", PhotonTargets.MasterClient);

		}
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer> ();

		pv = GetComponent<PhotonView> ();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		moveTowardCore ();
	}

	public void moveTowardCore() 
	{
		//only the master client moves enemies
		if (PhotonNetwork.isMasterClient) {
			Vector3 direction = corePosition - transform.position;
			rb.AddForce(direction.normalized * speed, ForceMode2D.Force);
			float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			rb.velocity = rb.velocity.magnitude > speed ? rb.velocity.normalized * speed : rb.velocity;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (PhotonNetwork.isMasterClient) {

			if (collision.gameObject.CompareTag("Core"))
			{

				//collision.gameObject.GetComponent<CoreBehaviour>().Damage(damage);
				var behaviour = collision.gameObject.GetComponent<CoreBehaviour>();
				if (null == behaviour) {
					collision.gameObject.GetComponent<RotatingCoreBehaviour> ().Damage(damage);

				} else {

					behaviour.Damage (damage);
				} 

				pv.RPC("DestroyLaser", PhotonTargets.MasterClient);
			}
		}
	}

	//triggered when this object is destroyed
	[PunRPC]
	public void DestroyLaser()
	{
		//not called through photon destroy
		if(PhotonNetwork.isMasterClient){
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
