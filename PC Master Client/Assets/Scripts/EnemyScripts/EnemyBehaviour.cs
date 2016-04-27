using UnityEngine;
using System.Collections;
using System;

public class EnemyBehaviour : MonoBehaviour
{
    public float speed;
    public int damage;
    public int totalHealth;
    public int points;
    int remainingHealth;

    Vector3 corePosition;
    Rigidbody2D rb;
    SpriteRenderer sprite;

    // Use this for initialization
    void Start()
    {
        try
        {
            corePosition = GameObject.FindGameObjectWithTag("Core").transform.position;
        }
        catch (NullReferenceException)
        {
            DestroyEnemy();

        }

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        remainingHealth = totalHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 direction = corePosition - transform.position;
        rb.AddForce(direction.normalized * speed, ForceMode2D.Force);
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rb.velocity = rb.velocity.magnitude > speed ? rb.velocity.normalized * speed : rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Core"))
        {

            //collision.gameObject.GetComponent<CoreBehaviour>().Damage(damage);
            var behaviour = collision.gameObject.GetComponent<CoreBehaviour>();
            if (null == behaviour)
            {
                collision.gameObject.GetComponent<RotatingCoreBehaviour>().Damage(damage);

            }
            else {

                behaviour.Damage(damage);
            }

            DestroyEnemy();
        }
    }


    public void decreaseHealth(int amount)
    {
        EnemyDamage(amount);
    }

    public void EnemyDamage(int amount)
    {
        remainingHealth -= amount;
        if (remainingHealth <= 0)
        {
            DestroyEnemy();
        }
        float healthPercent = (float)(totalHealth - remainingHealth) / totalHealth;
        sprite.color = new Color(1 - (float)Math.Pow(healthPercent, 2f), 0, 0);
    }

    //triggered when this object is destroyed
    void DestroyEnemy()
    {
        //not called through photon destroy
        //let enemy manager know when this enemy is dead
        Destroy(gameObject);
        EnemyManager manager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        manager.killEnemy();
    }
}

