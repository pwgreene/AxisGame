using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int speed;
	public float rotationSpeed;
	public int fireRate;

	float playerVertical;
	float playerHorizontal;
	bool playerFire;

	Rigidbody2D rb;

	int timeElapsedSinceFire;

	public GameObject laser;
	public Color color;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		playerVertical = 0;
		playerHorizontal = 0;
		timeElapsedSinceFire = 0;
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		if (sprite != null) {
			color = sprite.color;
		}
		print (color);
	}

	// Update is called once per frame
	void Update () 
	{
		playerHorizontal = Input.GetAxis("Horizontal");
		playerVertical = Input.GetAxis("Vertical");
		playerFire = Input.GetButtonDown("Fire1");
		if (timeElapsedSinceFire < fireRate) 
		{
			timeElapsedSinceFire++;
		}
	}

	void FixedUpdate()
	{
		rb.AddRelativeForce (new Vector2 (0, playerVertical * speed));
		rb.AddTorque (-playerHorizontal * rotationSpeed);
		if (playerFire) {
			Fire ();
		}
		print (timeElapsedSinceFire);
	}

	void Fire()
	{
		print ("fire");
		if (timeElapsedSinceFire == fireRate) {
			//create two new lasers to fire and set them equal to the color of the parent
			Vector3 rightBarrelEnd = transform.position +
				(Quaternion.Euler (transform.eulerAngles) * new Vector3 (.4f, 0, 0));
			Vector3 leftBarrelEnd = transform.position +
				(Quaternion.Euler (transform.eulerAngles) * new Vector3 (-.4f, 0, 0));
			GameObject rightLaser = Instantiate (laser, rightBarrelEnd, transform.rotation) as GameObject;
			rightLaser.GetComponent<SpriteRenderer> ().color = color;
			GameObject leftLaser = Instantiate (laser, leftBarrelEnd, transform.rotation) as GameObject;
			leftLaser.GetComponent<SpriteRenderer> ().color = color;
			timeElapsedSinceFire = 0;
		}
	}
}
