using UnityEngine;
using System.Collections;

public class HomingMissile : MonoBehaviour, Projectile {

	public int damage;
	public int life;
	Rigidbody2D rb;
	public int speed;
	public int max_turn;
	public TurretController owner;
	float turn_amount = 0;

	public float ROTATION_SCALAR;

	// Use this for initialization
	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		rb.AddRelativeForce (new Vector2 (0, speed));
	}
	
	// Update is called once per frame
	void Update () {
		if (life == 0) {
			Destroy (this.gameObject);

		} else {
			life--;
			GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");


			if (enemies.Length != 0) {

				//print ("Enemies detected: " + enemies.Length);

				GameObject closest = enemies [0];

				if (enemies.Length > 1) {
					foreach (GameObject enemy in enemies) {
						if (Vector3.Distance (this.gameObject.transform.position, enemy.transform.position) < Vector3.Distance (this.gameObject.transform.position, closest.transform.position)) {
							closest = enemy;
						}
					}

				}

				//print ("Closest enemy at: " + closest.transform.position);
				//print ("Bullet posiiton: " + this.gameObject.transform.position);
				//print ("Forward: " + this.gameObject.transform.forward);
				//print ("Vector to enemy: " + (closest.transform.position - this.gameObject.transform.position));
				//print ("RAGE: " + Vector2.Angle (closest.transform.position - this.gameObject.transform.position, Vector2.up));
				turn_amount = Vector2.Angle (closest.transform.position -this.gameObject.transform.position, this.gameObject.transform.up);

				Vector3 cross_product = Vector3.Cross (this.gameObject.transform.up, closest.transform.position - this.gameObject.transform.position);


				if (cross_product.z < 0) {
					//print ("Readjusting");
					turn_amount *= -1;
				}

				//print ("Angle: " + turn_amount);

			} else {
				//print ("No enemies");
			}

		}
	}

	void FixedUpdate(){
		if (Mathf.Abs (turn_amount) < max_turn) {
			//print ("Turning: " + turn_amount);
			//print("Turn: " + turn_amount);
			rb.AddTorque (turn_amount * ROTATION_SCALAR);
		} else {
			if (turn_amount < 0) {
				//print ("Limit Turning: " + max_turn);
				rb.AddTorque (-max_turn * ROTATION_SCALAR);
			} else {
				//print ("Limit Turning: " + -max_turn);
				rb.AddTorque (max_turn *ROTATION_SCALAR);
			}
		}
		rb.velocity.Normalize ();
		rb.AddRelativeForce(new Vector2(0, speed));
		//rb.velocity = Quaternion.AngleAxis (turn_amount, Vector3.forward) * this.gameObject.transform.up * speed;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (null != rb) {
			if (other.gameObject.CompareTag ("Enemy")) {

				EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
				//playerScript.IncreaseScore (enemyScript.points);
				if (owner.isControllable && enemyScript != null) {
					enemyScript.decreaseHealth (damage);
				}
				Destroy (gameObject);
			} else if(!other.gameObject.CompareTag("Player")){
				//print (other.relativeVelocity);
				Destroy (gameObject);
			}
		}


	}

	public int getDamage (){
		return damage;
	}

	public int getLife (){
		return life;
	} 

}
