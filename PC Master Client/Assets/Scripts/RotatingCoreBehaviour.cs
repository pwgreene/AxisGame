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

	void Start()
	{
		currentHP = startingHP;
		coreSprite = GetComponent<SpriteRenderer>();
	}

	void Update(){
		this.gameObject.transform.Rotate(new Vector3(0,0,rotationspeed));
	}

	[PunRPC]
	public void CoreDamage(int damageValue){
		currentHP -= (float)damageValue;
		coreSprite.color = new Color(1, 1 - (float)(startingHP - currentHP) / startingHP, 1 - (float)(startingHP - currentHP) / startingHP);

		if (currentHP < 0)
		{
			
			PhotonNetwork.Destroy(gameObject);
		}
	}

	public void Damage(int damageValue)
	{

		PhotonView photonView = PhotonView.Get(this);
		photonView.RPC("CoreDamage", PhotonTargets.All,damageValue);
	}
}