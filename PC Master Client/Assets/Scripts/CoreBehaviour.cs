using UnityEngine;
using System.Collections;

public class CoreBehaviour : MonoBehaviour
{

    public int startingHP;
    int currentHP;

    SpriteRenderer coreSprite;

    void Start()
    {
        currentHP = startingHP;
        coreSprite = GetComponent<SpriteRenderer>();
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
