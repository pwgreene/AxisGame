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
		gameObject.transform.Rotate(new Vector3(0,0,rotationspeed));
	}
    
	public void CoreDamage(int damageValue){
		currentHP -= damageValue;
		coreSprite.color = new Color(1, 1 - (startingHP - currentHP) / startingHP, 1 - (startingHP - currentHP) / startingHP);

		if (currentHP < 0)
		{
			Destroy(gameObject);
		}
	}

	public void Damage(int damageValue)
	{
        CoreDamage(damageValue);
	}
}