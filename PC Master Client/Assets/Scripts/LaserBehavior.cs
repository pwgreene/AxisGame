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

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag ("Enemy")) {
			PlayerController playerScript = owner.GetComponent<PlayerController> ();
			EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
			playerScript.IncreaseScore (enemyScript.points);
			if (enemyScript != null) {
				enemyScript.decreaseHealth (damage);
			}
			Destroy (gameObject);
		}/** else if (other.CompareTag ("Enemy2")) {
			PlayerController playerScript = owner.GetComponent<PlayerController> ();
			EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
			playerScript.IncreaseScore (enemyScript.points);
			if (enemyScript != null) {
				enemyScript.decreaseHealth (damage);
			}
			Destroy (gameObject);
		} else if (other.CompareTag ("Enemy3")) {
			PlayerController playerScript = owner.GetComponent<PlayerController> ();
			EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
			playerScript.IncreaseScore (enemyScript.points);
			if (enemyScript != null) {
				enemyScript.decreaseHealth (damage);
			}
			Destroy (gameObject);
		}**/
	}
}
