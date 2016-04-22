using UnityEngine;
using System.Collections;
using System;

public class mediumSuicider : EnemyBehaviour {

	public int totalHealth;
	public int points;
	public int damage;
	public int speed;

	int remainingHealth;
	Vector3 corePosition;
	Rigidbody2D rb;
	SpriteRenderer sprite;


	// Use this for initialization
	void Start () {
		remainingHealth = totalHealth;
		try
		{
			corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
		}
		catch (NullReferenceException)
		{
			Destroy(gameObject);
		}
		rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = corePosition - transform.position;
		rb.AddForce(direction.normalized * speed, ForceMode2D.Force);
		float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		rb.velocity = rb.velocity.magnitude > speed ? rb.velocity.normalized * speed : rb.velocity;
	}

	public void decreaseHealth(int amount) {
		remainingHealth -= amount;
		if (remainingHealth <= 0) {
			Destroy (gameObject);
		}
		float healthPercent = (float)(totalHealth - remainingHealth) / totalHealth;
		sprite.color = new Color(1 - (float)Math.Pow(healthPercent, 2f), 0, 0);
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Core"))
		{

			//collision.gameObject.GetComponent<CoreBehaviour>().Damage(damage);
			var behaviour = collision.gameObject.GetComponent<CoreBehaviour>();
			if (null == behaviour) {
				collision.gameObject.GetComponent<RotatingCoreBehaviour> ().Damage(damage);

			} else {

				behaviour.Damage (damage);
			} 

			Destroy(gameObject);
		}
	}

}
