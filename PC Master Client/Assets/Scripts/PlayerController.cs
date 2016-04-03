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

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		playerVertical = 0;
		playerHorizontal = 0;
		timeElapsedSinceFire = 0;
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
			Instantiate (laser, transform.position, transform.rotation);
			timeElapsedSinceFire = 0;
		}
	}
}
