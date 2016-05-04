using UnityEngine;
using System.Collections;

public class Grenade : MonoBehaviour, Projectile {

	public int damage;
	public int life;
	Rigidbody2D rb;
	public int speed;

	public int fragments;
	public GameObject grenadePiece;

	// Use this for initialization
	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
		rb.AddRelativeForce (new Vector2 (0, speed));
	}
	
	// Update is called once per frame
	void Update () {
	
		if (life == 0) {
			blowUp ();	
		} else {
			life--;
		}
	}

	void blowUp(){
		//Former code for individual grenade pieces
		/*for (int i = 0; i < fragments; i++) {
			GameObject piece = Instantiate (grenadePiece, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
			piece.transform.Rotate(new Vector3(0, 0, 360.0f / fragments * i));
		}*/

		GameObject explosion = Instantiate (grenadePiece, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
		explosion.GetComponent<GrenadeExplosion> ().owned = owned;
		explosion.GetComponent<SpriteRenderer> ().color = this.gameObject.GetComponent<SpriteRenderer> ().color;

		Destroy (this.gameObject);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (!other.gameObject.CompareTag("Bullet") && null != rb && null != owned){
			if (other.gameObject.CompareTag ("Enemy")) {

				EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
				//playerScript.IncreaseScore (enemyScript.points);
				if (owned && enemyScript != null) {
					enemyScript.decreaseHealth (damage);
				}
				blowUp ();
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

	public bool owned;

	public void setControllable(bool control){
		owned = control;
	}
	public int getDamage(){
		return damage;
	}
	public int getLife(){
		return life;
	}
}
