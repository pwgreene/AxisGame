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

		foreach(TurretController player in this.gameObject.GetComponentsInChildren<TurretController>()){
			Spoke a = (Spoke) Instantiate (rod, this.gameObject.transform.position, Quaternion.identity);
			a.core = this;
			a.player = player;
		}
	}

	void Update(){
		this.gameObject.transform.Rotate(new Vector3(0,0,rotationspeed));
	}

	public void Damage(int damageValue)
	{
		currentHP -= (float)damageValue;
		coreSprite.color = new Color(1, 1 - (float)(startingHP - currentHP) / startingHP, 1 - (float)(startingHP - currentHP) / startingHP);

		if (currentHP < 0)
		{
			Destroy(gameObject);
		}
	}
}