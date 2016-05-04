using UnityEngine;
using System.Collections;
using System;

public class EnemyLaserBehaviour : MonoBehaviour {

	public int damage;
	public int speed;

	Vector3 corePosition;

	public Rigidbody2D rb;

	void Start()
	{
		try
		{
			corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
		}
		catch (NullReferenceException)
		{
			PhotonView photonView = PhotonView.Get(this);
			DestroyLaser ();

		}
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		moveTowardCore ();
	}

	public void moveTowardCore() 
	{
		Vector3 direction = corePosition - transform.position;
		rb.AddForce(direction.normalized * speed, ForceMode2D.Force);
		float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		rb.velocity = rb.velocity.magnitude > speed ? rb.velocity.normalized * speed : rb.velocity;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		
		if (collision.gameObject.CompareTag("Core")) {
			if (PhotonNetwork.isMasterClient) {
				//collision.gameObject.GetComponent<CoreBehaviour>().Damage(damage);
				collision.gameObject.GetComponent<RotatingCoreBehaviour> ().Damage(damage);
			}
			Destroy (gameObject);
		} 
	}

	//triggered when this object is destroyed
	public void DestroyLaser()
	{
		Destroy (gameObject);
	}
}
