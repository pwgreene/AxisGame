using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	public GameObject first_button_object;
	public GameObject second_button_object;
	public GameObject third_button_object;
	public TurretController turret;

	// Use this for initialization
	void Start () {
		if (turret != null) {
			WeaponButton first_button = first_button_object.GetComponent<WeaponButton> ();
			first_button.player = turret;
			first_button.activation_num = 1;
			first_button.gameObject.SetActive (true);

			WeaponButton second_button = second_button_object.GetComponent<WeaponButton> ();
			second_button.player = turret;
			second_button.activation_num = 2;
			second_button.gameObject.SetActive (true);

			WeaponButton third_button = third_button_object.GetComponent<WeaponButton> ();
			third_button.player = turret;
			third_button.activation_num = 3;
			third_button.gameObject.SetActive (true);
		} else {
			print ("Don't have a player just yet.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (turret != null) {
			WeaponButton first_button = first_button_object.GetComponent<WeaponButton> ();
			first_button.player = turret;
			first_button.activation_num = 1;
			first_button.gameObject.SetActive (true);

			WeaponButton second_button = second_button_object.GetComponent<WeaponButton> ();
			second_button.player = turret;
			second_button.activation_num = 2;
			second_button.gameObject.SetActive (true);
			//second_button_object.GetComponent<Button> ().interactable = false;

			WeaponButton third_button = third_button_object.GetComponent<WeaponButton> ();
			third_button.player = turret;
			third_button.activation_num = 3;
			third_button.gameObject.SetActive (true);
			//third_button_object.GetComponent<Button> ().interactable = false;

			if(turret.ammoType == 0){
				first_button_object.GetComponent<Button> ().interactable = true;
				second_button_object.GetComponent<Button> ().interactable = false;
				third_button_object.GetComponent<Button> ().interactable = false;
			}
			if (turret.ammoType == 1) {
				first_button_object.GetComponent<Button> ().interactable = false;
				second_button_object.GetComponent<Button> ().interactable = true;
				third_button_object.GetComponent<Button> ().interactable = false;
			}
			if (turret.ammoType == 2) {
				first_button_object.GetComponent<Button> ().interactable = false;
				second_button_object.GetComponent<Button> ().interactable = false;
				third_button_object.GetComponent<Button> ().interactable = true;
			}
		}
	}
}
