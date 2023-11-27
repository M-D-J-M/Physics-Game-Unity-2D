using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaisyBossScript : MonoBehaviour
{
    // ENEMY FOLLOW //
    public GameObject player;
    public GameObject GameManager;
    public GameObject coin;
    public GameObject enemybullet;
    public GameObject grenade;

    public float moveSpeed = 2.0f;
    Rigidbody2D rb;
    public float maxattacktimer = 10.0f;
    float t;
    float attacktimer;
    public float attackRange = 50;

    //// HEALTH ////
    public GameObject HealthBar;
    public float maxhealth = 50;
    float currenthealth;
    float healthbarsize;

    //////////////// ATTACK/ SPRITE ///////////////
    SpriteRenderer sprite;
    public Transform firePoint;
    public float turnspeed = 0.5f;
    int randomattack;

    float steadyspeed = 50;
    float steadytorque = 50;
    float elapsed = 0f;
    float firerate = 0.1f;


    void Start()
    {
        t = maxattacktimer / 10;
        randomattack = Random.Range(1, 5);
        attacktimer = maxattacktimer;
        rb = GetComponent<Rigidbody2D>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        currenthealth = maxhealth;

        // Sprite //
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 1);
    }


    void Update()
    {
        float playerdistance = Vector3.Distance(player.transform.position, transform.position);

        if (playerdistance < attackRange) Attack();
    }


    void Attack()
    {
        attacktimer -= Time.deltaTime;

        if (attacktimer < ( 8 * t) && attacktimer > (5 * t))
        {
            Steady();
            sprite.color = new Color(1, 1, 1, 1); //white
        }

        else
        {
            if (randomattack == 1) { ShootAttack(); }
            if (randomattack == 2) { DashAttack(); }
            if (randomattack == 3) { GrenadeAttack(); }
            if (randomattack == 4) { SpinAttack(); }
        }
    }

    private void DashAttack()
    {
        if (attacktimer > 0 && attacktimer < (1 * t)) sprite.color = new Color(1, 0, 1, 1f); //magenta
        
        if (attacktimer <= 0)
        {
            rb.AddForce((player.transform.position - transform.position) * moveSpeed, ForceMode2D.Impulse);
            attacktimer = maxattacktimer;
            randomattack = Random.Range(1, 5);
        }
    }

    private void ShootAttack()
    {
        if (attacktimer > 4 * t && attacktimer < 5 * t)
        {
            sprite.color = new Color(0, 1, 1, 1); //cyan
            rb.AddTorque(-turnspeed * 2);
        }

        if (attacktimer < (4 * t) && attacktimer > (0 * t))
            elapsed += Time.deltaTime;
            if (elapsed >= firerate)
            {
                elapsed = elapsed % firerate;
                FireBullet(10, 4, enemybullet);
            }

        if (attacktimer <= 0)
        {
            randomattack = Random.Range(1, 5);
            attacktimer = maxattacktimer;
        }
    }

    private void SpinAttack()
    {
        if (attacktimer > 4 * t && attacktimer < 5 * t)
        {
            sprite.color = new Color(0.3f, 0.8f, 0.2f, 1); //cyan
            rb.AddTorque(-turnspeed * 15);
        }

        if (attacktimer < (4 * t) && attacktimer > (0 * t))
            elapsed += Time.deltaTime;
        if (elapsed >= firerate)
        {
            elapsed = elapsed % firerate;
            FireBullet(4, 10, enemybullet);
        }

        if (attacktimer <= 0)
        {
            randomattack = Random.Range(1, 5);
            attacktimer = maxattacktimer;
        }
    }


    void GrenadeAttack() 
    {
        if (attacktimer > 3 * t && attacktimer < 4 * t) sprite.color = new Color(0, 1, 0, 1); //green

        if (attacktimer <= 2 * t)
        {
            sprite.color = new Color(1, 1, 1, 1); //white
            FireBullet(10, 4, grenade);
            randomattack = Random.Range(1, 5);
            attacktimer = maxattacktimer;
        }
    }


    /////////// BULLET COLLIDE //////////////
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Bullet"))
        {
            AdjustHealth(1);
            GameManager.SendMessage("AddScore", SendMessageOptions.DontRequireReceiver);
        }
    }

    void AdjustHealth(float damage)
    {
        currenthealth -= damage;
        healthbarsize = currenthealth / maxhealth;
        HealthBar.transform.localScale = new Vector3(healthbarsize, 0.1f, 0);

        if (currenthealth <= 0)
        {
            // Spawn the bullet from the prefab.
            GameObject coin1 = Instantiate(coin, transform.position, Quaternion.identity);

            // Position of bullet to origin firepoint //
            coin1.transform.position = transform.position + new Vector3(0.0f, 0.0f, 0.0f);

            GameObject.Destroy(gameObject);
        }
    }

    void FireBullet(float speed,float bulletnumber, GameObject projectile)
    {
        float bulletanglegap = 360 / bulletnumber;
        
        for (float i = 0; i < bulletnumber; i++)
        {
            // Find start angle, for 90 spread , -45 degrees start
            float startangle = transform.rotation.z*100;

            // Adjust angle based on spread and number, for 7(-45,-30,-15,0,15,30,45)
            float fireangle = startangle + (bulletanglegap * i);

            // Reset firepoint rotation to desired angle.
            firePoint.rotation = Quaternion.AngleAxis(fireangle, Vector3.forward);

            // Spawn the bullet from the prefab.
            GameObject bulletClone = Instantiate(projectile, firePoint.position, Quaternion.identity);

            // Position of bullet to origin firepoint //
            bulletClone.transform.position = firePoint.position;

            // rotate bullet towards mouse, not that that really matters //
            bulletClone.transform.rotation = Quaternion.Euler(0, 0, fireangle);

            //float positionadjust = 2;
            bulletClone.transform.Translate(4, 0, 0);

            // fire bullet
            bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * speed;
        }
    }

    void Steady()
    {
        rb.angularVelocity -= rb.angularVelocity / steadytorque;
        rb.velocity -= rb.velocity / steadyspeed;

    }
}
