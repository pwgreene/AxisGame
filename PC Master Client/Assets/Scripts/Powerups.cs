using UnityEngine;
using System.Collections;

public class Powerups : MonoBehaviour {
	
	public PowerupType powType;

	public float percentageIncrease;
	public float scalarIncrease;

	private GameObject core;
	// Use this for initialization
	void Start () {

		//currently none of these effects are temporary

		//should be with find tag, but right now player is child of the core so they're all tagged core
		 core = GameObject.Find("rotating_core");
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Player") || other.tag.Equals("Core")) {
			//Debug.Log ("collided with powerup");

			switch (powType)
			{
			case PowerupType.CoreHealth:
				core.GetComponent<RotatingCoreBehaviour> ().currentHP = scalarIncrease + core.GetComponent<RotatingCoreBehaviour> ().currentHP* percentageIncrease; 
				break;
			case PowerupType.CoreRotationSpeed:
				core.GetComponent<RotatingCoreBehaviour> ().rotationspeed = scalarIncrease + core.GetComponent<RotatingCoreBehaviour> ().rotationspeed * percentageIncrease; 
				break;
			case PowerupType.FiringRate:
				other.GetComponent<TurretController> ().fireRate = scalarIncrease + other.GetComponent<TurretController> ().fireRate * percentageIncrease;
				break;
			case PowerupType.Speed:
				other.GetComponent<TurretController> ().speed = scalarIncrease + other.GetComponent<TurretController> ().speed * percentageIncrease;
				break;
			default:
				break;
			}


			//destroy this
			Destroy(transform.gameObject);
		}

	}
}

