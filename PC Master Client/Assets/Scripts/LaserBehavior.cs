using UnityEngine;
using System.Collections;

public class LaserBehavior : MonoBehaviour {

	Rigidbody2D rb;

	public int speed;
	public GameObject owner;
	public int life;
	public int damage;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		rb.AddRelativeForce (new Vector2(0, speed));
	}

	// Update is called once per frame
	void Update () {
		if (life == 0) {
			Destroy (gameObject);
		} else {
			life--;
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag ("Enemy")) {
			PlayerController playerScript = owner.GetComponent<PlayerController> ();
			EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
			playerScript.IncreaseScore (enemyScript.points);
			if (enemyScript != null) {
				enemyScript.decreaseHealth (damage);
			}
			Destroy (gameObject);
		} else if(!other.gameObject.CompareTag ("Player")){
			// get the point of contact
			ContactPoint2D contact = other.contacts[0];
			Vector3 oldVelocity = rb.velocity;
			// reflect our old velocity off the contact point's normal vector
			Vector3 reflectedVelocity = Vector3.Reflect(oldVelocity, contact.normal);        

			// assign the reflected velocity back to the rigidbody
			rb.velocity = reflectedVelocity;
			// rotate the object by the same ammount we changed its velocity
			Quaternion rotation = Quaternion.FromToRotation(oldVelocity, reflectedVelocity);
			transform.rotation = rotation * transform.rotation;
			//print (other.relativeVelocity);
		}
	}
}
