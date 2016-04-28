using UnityEngine;
using System.Collections;

public class ShieldBehaviour : MonoBehaviour {

	bool isActive;
	int timeActive;
	int timeInactive;

	public int activeLife;
	public int inactiveLife;

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		isActive = true;
		timeActive = 0;
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent != null) {
			transform.position = transform.parent.position;
		}
		//periodically go inactive and active
		if (isActive) {
			if (timeActive > activeLife) {
				GoInactive ();
			} else {
				timeActive++;
			} 
		} else {
			if (timeInactive > inactiveLife) {
				GoActive ();
			} else {
				timeInactive++;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Bullet") && isActive)
		{	
			Destroy (collision.gameObject);
		}
	}

	void GoActive() {
		sprite.enabled = true;
		timeActive = 0;
		isActive = true;
	}

	void GoInactive() {
		sprite.enabled = false;
		timeInactive = 0;
		isActive = false;
	}
}
