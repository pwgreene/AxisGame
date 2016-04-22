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
		explosion.GetComponent<SpriteRenderer> ().color = this.gameObject.GetComponent<SpriteRenderer> ().color;

		Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
			Destroy (other.gameObject);
			blowUp ();
		} else if (other.CompareTag ("Enemy2")) {
			Enemy2Behavior enemy = other.GetComponent<Enemy2Behavior> ();
			enemy.decreaseHealth (damage);
			blowUp();
		} else if (other.CompareTag ("Enemy3")) {
			Enemy3Behavior enemy = other.GetComponent<Enemy3Behavior> ();
			enemy.decreaseHealth (damage);
			blowUp();
		}

	}

	public int getDamage(){
		return damage;
	}
	public int getLife(){
		return life;
	}
}
