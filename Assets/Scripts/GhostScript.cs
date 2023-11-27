using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    // ENEMY FOLLOW //
    public GameObject player;
    public GameObject GameManager;
    public GameObject coin;

    public float moveSpeed = 1.0f;
    Rigidbody2D rb;
    public float timer = 2.0f;
    public float attackRange = 15;

    //// HEALTH ////
    public GameObject HealthBar;
    public float maxhealth = 3;
    float currenthealth;
    float damage = 1;
    float healthbarsize;

    //////////////// DAMAGE/SPRITE ///////////////
    SpriteRenderer sprite;
    float vunerabletimer;
    public float vunerablemaxtime = 2.0f;
    bool attacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        currenthealth = maxhealth;

        // Sprite //
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(1, 1, 1, 0.2f);
    }


    void Update()
    {
        // PLAYER DISTANCE //
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // TIMER //
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            //// ATTACK RANGE ////
            if (distance < attackRange) { Attack(); }
            timer = 3f;
        }

        AdjustColour();
    }

    private void Attack()
    {
        attacking = true;
        rb.AddForce((player.transform.position - transform.position) * moveSpeed, ForceMode2D.Impulse);
    }

    /////////// BULLET COLLIDE //////////////
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Bullet"))
        {
            AdjustHealth(damage);
            Debug.Log("Jumper ded");
            GameManager.SendMessage("AddScore", SendMessageOptions.DontRequireReceiver);
            //GameObject.Destroy(gameObject);
        }
    }

    void AdjustHealth(float damage)
    {
        if (attacking == true)
        {
            currenthealth -= damage;
            healthbarsize = currenthealth / maxhealth;
            HealthBar.transform.localScale = new Vector3(healthbarsize, 0.1f, 0);
        }

        if (currenthealth <= 0)
        {
            // Spawn the bullet from the prefab.
            GameObject smallswarmer1 = Instantiate(coin, transform.position, Quaternion.identity);

            // Position of bullet to origin firepoint //
            coin.transform.position = transform.position + new Vector3(0.0f, 0.0f, 0.0f);

            GameObject.Destroy(gameObject);
        }
    }

    void AdjustColour()
    {
        //Debug.Log(invunerabletimer);
        if (attacking)
        {

            vunerabletimer -= Time.deltaTime;
            sprite.color = new Color(1, 1, 1, 1f); //magenta

        }
        if (vunerabletimer <= 0)
        {
            vunerabletimer = vunerablemaxtime;
            attacking = false;
            sprite.color = new Color(1, 1, 1, 0.2f); // green

        }
    }
}
