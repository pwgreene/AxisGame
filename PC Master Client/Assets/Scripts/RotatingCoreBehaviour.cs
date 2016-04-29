using UnityEngine;
using System.Collections;

public class RotatingCoreBehaviour : MonoBehaviour
{

	public float startingHP;
	public float currentHP;
	public float rotationspeed;

	public float max_radius;

	const float LINE_WIDTH = .2F; 

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
		coreSprite.color = new Color(1, 1 - (float)(startingHP - currentHP) / startingHP, 1 - (float)(startingHP - currentHP) / startingHP);

		if (currentHP < 0)
		{
			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.Destroy(gameObject);
			}

		}
	}

	public void Damage(int damageValue)
	{


		pv.RPC("CoreDamage", PhotonTargets.AllBufferedViaServer,damageValue);
	}
}