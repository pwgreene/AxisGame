using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour {

	//Number used to activate this button
	public int activation_num;

	//The player for which we are displaying the information
	public TurretController player;
	//public string ammo_remaining;

	// Use this for initialization
	void Start () {

		this.gameObject.transform.FindChild ("activate_button").GetComponent<Text> ().text = ""+activation_num;
		this.gameObject.transform.FindChild ("ammo_icon").GetComponent<Image> ().sprite = player.weapons [activation_num - 1].GetComponent<SpriteRenderer> ().sprite;
		this.gameObject.transform.FindChild ("ammo_left").GetComponent<Text> ().text = ""+player.ammoAmmounts [activation_num - 1];
	}
	
	// Update is called once per frame
	void Update () {
		if (player.ammoAmmounts [activation_num - 1] == Mathf.Infinity) {
			//Infinity Character
			this.gameObject.transform.FindChild ("ammo_left").GetComponent<Text> ().text = "8";
			this.gameObject.transform.FindChild("ammo_left").transform.localEulerAngles = new Vector3 (0, 0, 90);
		} else {
			this.gameObject.transform.FindChild ("ammo_left").GetComponent<Text> ().text = ""+player.ammoAmmounts [activation_num - 1];
		}
		 
	}
}
