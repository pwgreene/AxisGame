using UnityEngine;
using System.Collections;

public class GrenadeExplosion : MonoBehaviour, Projectile {

	public float expansion_speed;
	public int life;
	public int damage;
	public TurretController owner;
	CircleCollider2D collider_object;
	SpriteRenderer explosion_sprite;

	// Use this for initialization
	void Start () {
		collider_object = this.gameObject.GetComponent<CircleCollider2D> ();
		explosion_sprite = this.gameObject.GetComponent<SpriteRenderer> ();

		if (collider_object == null || explosion_sprite == null) {
			print ("There is a problem with grenade explosion");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (life == 0) {
			Destroy (this.gameObject);
		} else {
			//collider_object.radius += expansion_speed;
			explosion_sprite.transform.localScale += new Vector3 (expansion_speed, expansion_speed, 0);
			life--;
		}
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (null != owner)
			//print ("Collision");
			if (other.CompareTag ("Enemy")) {
				EnemyBehaviour enemyScript = other.gameObject.GetComponent<EnemyBehaviour> ();
				//owner.IncreaseScore (enemyScript.points);
				if (owner.isControllable && enemyScript != null) {
					enemyScript.decreaseHealth (damage);
				}
			}/** else if (other.CompareTag ("Enemy2")) {
				Enemy2Behavior enemy = other.GetComponent<Enemy2Behavior> ();
				enemy.decreaseHealth (damage);
			} else if (other.CompareTag ("Enemy3")) {
				Enemy3Behavior enemy = other.GetComponent<Enemy3Behavior> ();
				enemy.decreaseHealth (damage);
			}**/
		}
	}

	public int getLife(){
		return life;
	}

	public int getDamage(){
		return damage;
	}

}
