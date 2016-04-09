using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {

	public float speed;
	public float rotationSpeed;
	public int fireRate;

	float playerVertical;
	float playerHorizontal;
	bool playerFire;

	Rigidbody2D rb;

	GameObject player;

	Animator left_turret;
	Transform left_turret_position;

	Animator right_turret;
	Transform right_turret_position;

	int timeElapsedSinceFire;

	public GameObject laser;
	public Color color;

	bool leftShooting = true;
	bool rightShooting = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		player = this.gameObject;


		left_turret = player.transform.FindChild ("turret_left").GetComponent<Animator>();
		left_turret_position = player.transform.FindChild ("turret_left").transform;

		right_turret = player.transform.FindChild ("turret_right").GetComponent<Animator>();
		right_turret_position = player.transform.FindChild ("turret_right").transform;

		playerVertical = 0;
		playerHorizontal = 0;
		timeElapsedSinceFire = 0;
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		if (sprite != null) {
			color = sprite.color;
		}
		if (right_turret != null) {
			print ("We have found the right turret");
		}
		if (left_turret != null) {
			print ("We have found the left turret");
		}
		print (color);
	}

	// Update is called once per frame
	void Update () 
	{
		playerHorizontal = Input.GetAxis("Horizontal");
		playerVertical = Input.GetAxis("Vertical");
		playerFire = Input.GetButton("Fire1");
		if (timeElapsedSinceFire < fireRate) 
		{
			timeElapsedSinceFire++;
		}
	}

	void FixedUpdate()
	{
		Vector3 toCenter = (this.transform.position + this.transform.parent.position).normalized;
		Vector3 distance = toCenter * playerVertical * speed;

		if (this.GetComponent<CircleCollider2D> ().radius + this.transform.parent.GetComponent<CircleCollider2D> ().radius >
		    Vector3.Distance (this.gameObject.transform.position + distance, this.transform.parent.position)) {
			print ("Collision");
			this.gameObject.transform.position = toCenter * (this.gameObject.transform.parent.GetComponent<CircleCollider2D> ().radius + this.GetComponent<CircleCollider2D> ().radius);
		}
		else {
			this.gameObject.transform.position += toCenter * playerVertical * speed;
		}



		//print (toCenter * playerVertical * speed);
		//print ("Vetical: " + playerVertical);
			//new Vector2 (this.gameObject.transform.position.x-this.gameObject.transform.parent.transform.position.x,
			//	this.gameObject.transform.position.y-this.gameObject.transform.parent.transform.position.y)*playerVertical*speed)
		//rb.AddRelativeForce (new Vector2 (0, playerVertical * speed));
		rb.AddTorque (-playerHorizontal * rotationSpeed);
		if (playerFire) {
			Fire ();
		}
		//print (timeElapsedSinceFire);
	}

	void OnCollisionEnter(Collision col){
		print ("We have a collision");
		if (col.gameObject.name == "rotating_core") {
			print ("Core collision");
		}
	}

	void Fire()
	{
		print ("fire");
		if (timeElapsedSinceFire == fireRate) {
			//create two new lasers to fire and set them equal to the color of the parent
			if (leftShooting) {
				leftShooting = false;
				rightShooting = true;
				Vector3 leftBarrelEnd = left_turret_position.position;

				left_turret.SetTrigger ("Recoil");

				GameObject leftLaser = Instantiate (laser, leftBarrelEnd, transform.rotation) as GameObject;
				leftLaser.GetComponent<SpriteRenderer> ().color = color;

				
			} else if (rightShooting) {
				rightShooting = false;
				leftShooting = true;
				Vector3 rightBarrelEnd = right_turret_position.position;

				right_turret.SetTrigger("Recoil");
				GameObject rightLaser = Instantiate (laser, rightBarrelEnd, transform.rotation) as GameObject;
				rightLaser.GetComponent<SpriteRenderer> ().color = color;
			}




			timeElapsedSinceFire = 0;
		}
	}
}