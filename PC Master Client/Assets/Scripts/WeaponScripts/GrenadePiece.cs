using UnityEngine;
using System.Collections;

public class GrenadePiece : MonoBehaviour, Projectile {

	public int damage;
	public int life;
	Rigidbody2D rb;
	public int speed;

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
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
			Destroy (other.gameObject);
			Destroy (gameObject);
		} else if (other.CompareTag ("Enemy2")) {
			Enemy2Behavior enemy = other.GetComponent<Enemy2Behavior> ();
			enemy.decreaseHealth (damage);
			Destroy (gameObject);
		} else if (other.CompareTag ("Enemy3")) {
			Enemy3Behavior enemy = other.GetComponent<Enemy3Behavior> ();
			enemy.decreaseHealth (damage);
			Destroy (gameObject);
		}

	}

	public int getLife(){
		return life;
	}

	public int getDamage(){
		return damage;
	}
}
