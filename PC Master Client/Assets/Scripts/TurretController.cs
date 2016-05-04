using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour {
	Color[] playerColors = new Color[]{Color.cyan, Color.green, Color.magenta, Color.blue};

	public Color playerColor;
	public bool isControllable = false;
	public float speed;
	public float rotationSpeed;
	public float fireIntervalSeconds;
	public float orbiting_speed;


	float BASE_FIRE_INTERVAL;
	public bool increased_fire_rate = false;
	public float increased_fire_rate_duration = 0;


	public float max_distance;

	float playerVertical;
	float playerHorizontal;
	bool playerFire;

	Rigidbody2D rb;

	GameObject player;

	Animator left_turret;
	Transform left_turret_position;

	Animator right_turret;
	Transform right_turret_position;

	float timeElapsedSinceFire;


	public GameObject[] weapons;


	public GameObject spoke_object;

	bool leftShooting = true;
	bool rightShooting = false;

	//public bool hasSpoke = false;
    int ammoType;

	public float[] ammoAmmounts;

	public float coreRadius = 1.845f;
	SpriteRenderer sprite;
	PhotonView pv;
	// Use this for initialization
	void Start () {

		ammoAmmounts = new float[weapons.Length];
		ammoAmmounts [0] = Mathf.Infinity;
		ammoAmmounts [1] = 30;
		ammoAmmounts [2] = 30;

		ammoType = 0;

		BASE_FIRE_INTERVAL= fireIntervalSeconds;

		rb = GetComponent<Rigidbody2D>();
		player = this.gameObject;

		left_turret = player.transform.FindChild ("turret_left").GetComponent<Animator>();
		left_turret_position = player.transform.FindChild ("turret_left").transform;

		right_turret = player.transform.FindChild ("turret_right").GetComponent<Animator>();
		right_turret_position = player.transform.FindChild ("turret_right").transform;

		playerVertical = 0;
		playerHorizontal = 0;
		timeElapsedSinceFire = 0;
		 sprite = GetComponent<SpriteRenderer> ();
		if (sprite != null) {
			playerColor = sprite.color;
		}
		if (right_turret != null) {
			print ("We have found the right turret");
		}
		if (left_turret != null) {
			print ("We have found the left turret");
		}
		//print (color);

		//core = GameObject.FindGameObjectWithTag ("Core").transform;

		GameObject a = Instantiate (spoke_object, Vector3.zero, Quaternion.identity) as GameObject;
		Spoke spoke = a.GetComponent<Spoke>();
		spoke.player = this;
		//spoke.core = core.gameObject.GetComponent<RotatingCoreBehaviour>();

		pv = PhotonView.Get(this);

		if (isControllable) {
			pv.RPC("SetColor", PhotonTargets.AllBuffered, PhotonNetwork.playerList.Length-1);

		}
	}

	[PunRPC]
	public void PowerUpApply(bool inc_rate, int fire_interval, float dur,int ammo,float ammo_increase){
		if (inc_rate) {
			increased_fire_rate = true;
			fireIntervalSeconds = fire_interval;

		}

		increased_fire_rate_duration += dur;
		ammoAmmounts [ammo] += ammo_increase;
	}
	IEnumerator ColorUpdate(int ID){
		
		yield return new WaitForSeconds(0.5f);
		//some players might get the same color if the player leaves and joins
		playerColor = playerColors[ID];
		if (null != sprite) {
			sprite = GetComponent<SpriteRenderer> ();
		}
		sprite.color = playerColor;
		SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sp in sprites) {
			sp.color = playerColor;
		}

	}
	// Update is called once per frame
	void Update () 
	{
		if (isControllable) {
			playerHorizontal = Input.GetAxis("Horizontal");
			playerVertical = Input.GetAxis("Vertical");
			playerFire = Input.GetButton("Fire1");

			if(Input.GetKeyDown("1") && ammoAmmounts[0] > 0){
				ammoType = 0;
			}
			if (Input.GetKeyDown ("2") && ammoAmmounts[1] > 0) {
				ammoType = 1;
			}
			if (Input.GetKeyDown ("3") && ammoAmmounts[2] > 0) {
				ammoType = 2;
			}

			if (increased_fire_rate) {
				if (increased_fire_rate_duration <= 0) {
					increased_fire_rate_duration = 0;
					increased_fire_rate = false;
					fireIntervalSeconds = BASE_FIRE_INTERVAL;
					print ("Returning to base fireRate");
				} else {
					increased_fire_rate_duration--;
					print ("Countdown: " + increased_fire_rate_duration);
				}
			}
		}
			
	
			
		

	}

	void orbit (){
		if (isControllable) {

			Vector3 new_distance =  Quaternion.AngleAxis (orbiting_speed, Vector3.forward) * (this.gameObject.transform.position);
			this.gameObject.transform.position = new_distance;
			Vector3 current_rotation = this.gameObject.transform.rotation.eulerAngles;
			current_rotation = new Vector3 (current_rotation.x, current_rotation.y, current_rotation.z + orbiting_speed);
			this.gameObject.transform.rotation = Quaternion.Euler (current_rotation);
		}
	}

	void FixedUpdate()
	{
		
		if (isControllable) {
			//assuming 60 fps
			if (timeElapsedSinceFire < fireIntervalSeconds ) 
			{
				timeElapsedSinceFire += Time.deltaTime;
			}
			Vector3 toCenter = (this.transform.position + Vector3.zero).normalized;
			Vector3 distance = toCenter * playerVertical * speed;
			float dot_product = Vector3.Dot ((Vector3.zero + (this.gameObject.transform.position + toCenter * playerVertical * speed)).normalized, toCenter);

			//print ("Player Vertical: " + playerVertical);

			//print ("Dot: " + dot_product);

			if (this.GetComponent<CircleCollider2D> ().radius + coreRadius>
				Vector3.Distance (this.gameObject.transform.position + distance, Vector3.zero) ||
				playerVertical < 0.0f && -1.01f < dot_product && dot_product < -.99f) {
				print ("Collision");
				//this.gameObject.transform.position = toCenter * (core.gameObject.GetComponent<CircleCollider2D> ().radius + this.GetComponent<CircleCollider2D> ().radius);
			} 
			else if (playerVertical > 0.0f && Vector3.Distance (this.gameObject.transform.position + distance, Vector3.zero) > max_distance) {
				print ("Outer limit Reached");
				//this.gameObject.transform.position = toCenter * max_distance;

			}
			else {
				this.gameObject.transform.position += toCenter * playerVertical * speed;
			}

			//print("Distance from core: " + Vector3.Distance(this.gameObject.transform.position, core.position));

			//print (toCenter * playerVertical * speed);
			//print ("Vetical: " + playerVertical);
			//new Vector2 (this.gameObject.transform.position.x-this.gameObject.transform.parent.transform.position.x,
			//	this.gameObject.transform.position.y-this.gameObject.transform.parent.transform.position.y)*playerVertical*speed)
			//rb.AddRelativeForce (new Vector2 (0, playerVertical * speed));
			rb.AddTorque (-playerHorizontal * rotationSpeed);

			//If we have ammo and the player pressed the fire button
			if (playerFire && ammoAmmounts[ammoType] > 0) {



				if (timeElapsedSinceFire >= fireIntervalSeconds) {
					pv.RPC ("Fire", PhotonTargets.All, ammoType);
					timeElapsedSinceFire = 0;
				}


				//If we run out of ammo in our current weapon, go back to basic laser
				if (ammoAmmounts [ammoType] <= 0) {
					ammoType = 0;
					print ("Switching back to laser");
				}
			}
			orbit ();
		}

		//print (timeElapsedSinceFire);
	}


	public void setControllable(bool val){
		isControllable = val;

		//transform.FindChild ("Camera").gameObject.SetActive (true);
		//Camera.main.enabled = false;
		//GetComponentInChildren<Camera> ().enabled = true;
		GetComponentInChildren<AudioListener>().enabled = true;


	}

		

	void OnCollisionEnter(Collision col){
		print ("We have a collision");
		if (col.gameObject.name == "rotating_core") {
			print ("Core collision");
		}
	}
	[PunRPC]
	void SetColor(int ID){
		StartCoroutine ("ColorUpdate", ID);

	}
	[PunRPC]
	void Fire(int ammo_num)
	{
		ammoAmmounts [ammoType]--;
		print ("Ammotype: " + ammoType + " ammoLeft: " + ammoAmmounts [ammoType]);

				
		GameObject laser; 
		Vector3 barrelEnd = new Vector3(0,0,0);
		//create two new lasers to fire and set them equal to the color of the parent
		if (leftShooting) {
			leftShooting = false;
			rightShooting = true;
			barrelEnd = left_turret_position.position;

			left_turret.SetTrigger ("Recoil");
			laser = Instantiate (weapons[ammo_num], barrelEnd, transform.rotation) as GameObject;

			laser.GetComponent<SpriteRenderer> ().color = playerColor;
			switch (ammo_num) {
			case 0:
				LaserBehavior a = laser.GetComponent<LaserBehavior> () as LaserBehavior;
				a.owned = isControllable;
				break;
			case 1:
				HomingMissile b = laser.GetComponent<HomingMissile> () as HomingMissile;
				b.owned = isControllable;
				break;

			case 2:

				Grenade c = laser.GetComponent<Grenade> () as Grenade;
				c.owned = isControllable;
				break;
			}
		} else if (rightShooting) {
			rightShooting = false;
			leftShooting = true;
			barrelEnd = right_turret_position.position;

			right_turret.SetTrigger("Recoil");
			laser = Instantiate (weapons[ammo_num], barrelEnd, transform.rotation) as GameObject;
			laser.GetComponent<SpriteRenderer> ().color = playerColor;
			switch (ammo_num) {
			case 0:
				LaserBehavior a = laser.GetComponent<LaserBehavior> () as LaserBehavior;
				a.owned = isControllable;
				break;
			case 1:
				HomingMissile b = laser.GetComponent<HomingMissile> () as HomingMissile;
				b.owned = isControllable;
				break;

			case 2:

				Grenade c = laser.GetComponent<Grenade> () as Grenade;
				c.owned = isControllable;
				break;
			}
		}
	






	}
}