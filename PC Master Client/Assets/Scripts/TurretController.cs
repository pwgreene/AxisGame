using UnityEngine;
using System.Collections;

public class TurretController : MonoBehaviour
{

    public bool isControllable = false;
    public float speed;
    public float rotationSpeed;
    public float fireRate;
    public float orbiting_speed;

    public float max_distance;

    float playerVertical;
    float playerHorizontal;
    bool playerFire;

    Rigidbody2D rb;

    GameObject player;

    Animator left_turret;
    Transform left_turret_position;

    Animator right_turret;
    Transform right_turret_position;

    int timeElapsedSinceFire;
    public Color color;

    public GameObject[] weapons;

    public GameObject spoke_object;

    bool leftShooting = true;
    bool rightShooting = false;

    //public bool hasSpoke = false;
    int ammoType;

    Transform core;

    // Use this for initialization
    void Start()
    {


        ammoType = 0;

        rb = GetComponent<Rigidbody2D>();
        player = gameObject;


        left_turret = player.transform.FindChild("turret_left").GetComponent<Animator>();
        left_turret_position = player.transform.FindChild("turret_left").transform;

        right_turret = player.transform.FindChild("turret_right").GetComponent<Animator>();
        right_turret_position = player.transform.FindChild("turret_right").transform;

        playerVertical = 0;
        playerHorizontal = 0;
        timeElapsedSinceFire = 0;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            color = sprite.color;
        }
        if (right_turret != null)
        {
            print("We have found the right turret");

        }
        if (left_turret != null)
        {
            print("We have found the left turret");
        }

        core = GameObject.FindGameObjectWithTag("Core").transform;

        GameObject a = Instantiate(spoke_object, core.position, Quaternion.identity) as GameObject;
        Spoke spoke = a.GetComponent<Spoke>();
        spoke.player = this;
        spoke.core = core.gameObject.GetComponent<RotatingCoreBehaviour>();

        setControllable(isControllable);
    }

    public void SetHorizontal(float horizontal)
    {
        playerHorizontal = horizontal;
    }

    public void SetVertical(float vertical)
    {
        playerVertical = vertical;
    }

    public void SetWeapon(int ammoType)
    {
        if (weapons[this.ammoType] != null)
        {
            this.ammoType = ammoType;
        }
    }

    public void SetFiring(int firing)
    {
        if (firing == 0)
        {
            playerFire = false;
        } else
        {
            playerFire = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isControllable)
        {
            /*playerHorizontal = Input.GetAxis("Horizontal");
            playerVertical = Input.GetAxis("Vertical");
            playerFire = Input.GetButton("Fire1");

            if (Input.GetKeyDown("1"))
            {
                ammoType = 0;
            }
            if (Input.GetKeyDown("2"))
            {
                ammoType = 1;
            }
            if (Input.GetKeyDown("3"))
            {
                ammoType = 2;
            }*/

            if (timeElapsedSinceFire < fireRate)
            {
                timeElapsedSinceFire++;
            }
        }

        Orbit();
    }

    void Orbit()
    {
        Vector3 new_distance = Quaternion.AngleAxis(orbiting_speed, Vector3.forward) * (gameObject.transform.position);
        gameObject.transform.position = new_distance;
        Vector3 current_rotation = gameObject.transform.rotation.eulerAngles;
        current_rotation = new Vector3(current_rotation.x, current_rotation.y, current_rotation.z + orbiting_speed);
        gameObject.transform.rotation = Quaternion.Euler(current_rotation);
    }

    void FixedUpdate()
    {
        if (isControllable)
        {
            Vector3 toCenter = (transform.position + core.position).normalized;
            Vector3 distance = toCenter * playerVertical * speed;
            float dot_product = Vector3.Dot((core.position + (gameObject.transform.position + toCenter * playerVertical * speed)).normalized, toCenter);
            
            if (GetComponent<CircleCollider2D>().radius + core.gameObject.GetComponent<CircleCollider2D>().radius >
                Vector3.Distance(gameObject.transform.position + distance, core.position) ||
                playerVertical < 0.0f && -1.01f < dot_product && dot_product < -.99f)
            {
                print("Collision");
            }
            else if (playerVertical > 0.0f && Vector3.Distance(gameObject.transform.position + distance, core.position) > max_distance)
            {
                print("Outer limit Reached");

            }
            else {
                gameObject.transform.position += toCenter * playerVertical * speed;
            }

            rb.AddTorque(-playerHorizontal * rotationSpeed);
            if (playerFire)
            {
                Fire();
            }
        }
    }

    public void setControllable(bool val)
    {
        isControllable = val;

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (val == true)
        {
            foreach (SpriteRenderer spr in sprites)
            {
                Color color = spr.color;
                color.a = 1f;
                spr.color = color;
            }
        } else
        {
            foreach (SpriteRenderer spr in sprites)
            {
                Color color = spr.color;
                color.a = 0.25f;
                spr.color = color;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        print("We have a collision");
        if (col.gameObject.name == "rotating_core")
        {
            print("Core collision");
        }
    }

    public bool Fire()
    {
        if (timeElapsedSinceFire >= fireRate)
        {
            timeElapsedSinceFire = 0;

            //create two new lasers to fire and set them equal to the color of the parent
            if (leftShooting)
            {
                leftShooting = false;
                rightShooting = true;
                Vector3 leftBarrelEnd = left_turret_position.position;

                left_turret.SetTrigger("Recoil");

                GameObject leftLaser = Instantiate(weapons[ammoType], leftBarrelEnd, transform.rotation) as GameObject;
                leftLaser.GetComponent<SpriteRenderer>().color = color;
            }
            else if (rightShooting)
            {
                rightShooting = false;
                leftShooting = true;
                Vector3 rightBarrelEnd = right_turret_position.position;

                right_turret.SetTrigger("Recoil");
                GameObject rightLaser = Instantiate(weapons[ammoType], rightBarrelEnd, transform.rotation) as GameObject;
                rightLaser.GetComponent<SpriteRenderer>().color = color;
            }

            return true;
        }

        return false;
    }
}