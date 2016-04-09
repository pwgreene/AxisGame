using UnityEngine;
using System.Collections;

public class RotatingCoreBehaviour : MonoBehaviour
{

	public int startingHP;
	int currentHP;
	public int rotationspeed;

	SpriteRenderer coreSprite;

	void Start()
	{
		currentHP = startingHP;
		coreSprite = GetComponent<SpriteRenderer>();
	}

	void Update(){
		this.gameObject.transform.Rotate(new Vector3(0,0,rotationspeed));
	}

	public void Damage(int damageValue)
	{
		currentHP -= damageValue;
		coreSprite.color = new Color(1, 1 - (float)(startingHP - currentHP) / startingHP, 1 - (float)(startingHP - currentHP) / startingHP);

		if (currentHP < 0)
		{
			Destroy(gameObject);
		}
	}
}