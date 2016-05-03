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
			
			PhotonView photonView = PhotonView.Get(this);
			photonView.RPC("DestroyEnemy", PhotonTargets.MasterClient);

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

				//collision.gameObject.GetComponent<CoreBehaviour>().Damage(damage);
				var behaviour = collision.gameObject.GetComponent<CoreBehaviour>();
				if (null == behaviour) {
					collision.gameObject.GetComponent<RotatingCoreBehaviour> ().Damage(damage);

				} else {

					behaviour.Damage (damage);
				} 

				pv.RPC("DestroyEnemy", PhotonTargets.MasterClient);
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
			
			pv.RPC("DestroyEnemy", PhotonTargets.MasterClient);
			GameObject ex = Instantiate (explosion, transform.position, transform.rotation) as GameObject;
			ParticleSystem pEx = ex.GetComponent<ParticleSystem> ();
			var pEm = pEx.emission;
			pEm.rate = new ParticleSystem.MinMaxCurve ((float) (points /2));
		}
		float healthPercent = (float)(totalHealth - remainingHealth) / totalHealth;
		sprite.color = new Color(1 - (float)Math.Pow(healthPercent, 2f), 0, 0);
	}

	//triggered when this object is destroyed
	[PunRPC]
	public void DestroyEnemy()
	{
		//not called through photon destroy
		//let enemy manager know when this enemy is dead
		//if (transform.parent != null) {
		if(PhotonNetwork.isMasterClient){
			PhotonNetwork.Destroy(gameObject);
			EnemyManager manager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager> ();
			manager.killEnemy ();

		}

	}
}
