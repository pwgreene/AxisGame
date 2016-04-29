using UnityEngine;
using System.Collections;

public class ShieldBehaviour : MonoBehaviour {

	public bool isActive;
	public int timer;


	public int activeLife;
	public int inactiveLife;
	PhotonView pv;
	public SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		isActive = true;

		timer = 0;
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		if (sprite == null) {
			Destroy (gameObject);
		}

		pv = GetComponentInParent<PhotonView> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.parent != null) {
			transform.position = transform.parent.position;
		}
		if (PhotonNetwork.isMasterClient) {

			//periodically go inactive and active
			if (isActive) {
				if (timer > activeLife) {
					//GoInactive ();

					pv.RPC ("SetShieldActive", PhotonTargets.AllBufferedViaServer, !isActive);
				} else {
					timer++;
				} 
			} else {
				if (timer > inactiveLife) {
					//GoActive ();
					pv.RPC ("SetShieldActive", PhotonTargets.AllBufferedViaServer, !isActive);
				} else {
					timer++;
				}
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




}
