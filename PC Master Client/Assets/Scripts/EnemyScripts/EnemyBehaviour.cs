using UnityEngine;
using System.Collections;
using System;

public class EnemyBehaviour : MonoBehaviour
{

    public float speed;
    public int damage;
	public int totalHealth;
	public int points;
	public int remainingHealth;

	public GameObject explosion;
	public PhotonView pv;
    public Vector3 corePosition;
    Rigidbody2D rb;
	SpriteRenderer sprite;

    // Use this for initialization
    protected virtual void Start()
    {
        try
        {
            corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
        }
        catch (NullReferenceException)
        {
			

			DestroyEnemy ();

        }
        rb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer> ();
		remainingHealth = totalHealth;

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

				collision.gameObject.GetComponent<RotatingCoreBehaviour> ().Damage(damage);

		
				DestroyEnemy ();
			}
		}
    }


	public void decreaseHealth(int amount){
		
		pv.RPC("EnemyDamage", PhotonTargets.AllBufferedViaServer,amount);

	}

	[PunRPC]
	public void EnemyDamage(int amount) {
		remainingHealth -= amount;
		if (remainingHealth <= 0) {
			//this instantiate is outside so everyone sees it
			Instantiate (explosion, transform.position, transform.rotation);
			//not called through photon destroy
			//let enemy manager know when this enemy is dead
			//if (transform.parent != null) {

			DestroyEnemy ();
//			GameObject ex = Instantiate (explosion, transform.position, transform.rotation) as GameObject;
//			ParticleSystem pEx = ex.GetComponent<ParticleSystem> ();
//			var pEm = pEx.emission;
//			pEx.Emit (15);
		}
		float healthPercent = (float)(totalHealth - remainingHealth) / totalHealth;
		sprite.color = new Color(1 - (float)Math.Pow(healthPercent, 2f), 0, 0);
	}
		

	void DestroyEnemy(){
		if(PhotonNetwork.isMasterClient){
			PhotonNetwork.Destroy(gameObject);
			EnemyManager manager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager> ();
			manager.killEnemy ();

		}
	}
}
