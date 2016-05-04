using UnityEngine;
using System.Collections;

public class RotatingCoreBehaviour : MonoBehaviour
{

	public float startingHP;
	public float currentHP;
	public float rotationspeed;

	public float max_radius;

	const float LINE_WIDTH = .2F; 
	public GameObject coreExplosion;
	public Spoke rod;

	SpriteRenderer coreSprite;
	PhotonView pv;
	void Start()
	{
		currentHP = startingHP;
		coreSprite = GetComponent<SpriteRenderer>();
		pv = PhotonView.Get(this);
	}

	void FixedUpdate(){
		this.gameObject.transform.Rotate(new Vector3(0,0,rotationspeed));
	}

	[PunRPC]
	public void CoreDamage(int damageValue){
		currentHP -= (float)damageValue;
		if (null == coreSprite) {
			coreSprite = GetComponent<SpriteRenderer>();
		} 
		coreSprite.color = new Color(1, 1 - (float)(startingHP - currentHP) / startingHP, 1 - (float)(startingHP - currentHP) / startingHP);

		if (currentHP < 0)
		{
			EndGame ();

			if (PhotonNetwork.isMasterClient) {
				
				PhotonNetwork.Destroy(gameObject);
			}

		}
	}


	public void EndGame(){
		Instantiate (coreExplosion, transform.position, transform.rotation);
		GameObject gui = GameObject.FindGameObjectWithTag ("GUIManager");
		gui.GetComponent<GUIManager> ().EndScreen ();
	}
	public void Damage(int damageValue)
	{
		if (null == pv) {
			pv = PhotonView.Get(this);
		}

		pv.RPC("CoreDamage", PhotonTargets.AllBufferedViaServer,damageValue);
	}
}