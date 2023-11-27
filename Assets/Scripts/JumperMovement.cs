using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperMovement : MonoBehaviour
{
    // ENEMY FOLLOW //
    public GameObject player;
    public GameObject GameManager;
    public float moveSpeed = 50.0f;
    Rigidbody2D rb;
    public float timer = 2.0f;
    public float attackRange = 15;

    //// HEALTH ////
    public GameObject HealthBar;
    public float maxhealth = 3;
    float currenthealth;
    float damage = 1;
    float healthbarsize;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        currenthealth = maxhealth;
    }


    void Update()
    {
        /////////  PLAYER DISTANCE ///////// 

        float distance = Vector3.Distance(player.transform.position, transform.position);

        /////////////  TIMER ///////////// 

        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }

        ///////// ATTACK RANGE //////////
        
        if (timer <= 0)
        {
            if (distance < attackRange) { Attack(); }
            timer = 2f;
        }
    }

    private void Attack()
    {
        rb.AddForce((player.transform.position - transform.position) * 100f);
    }


    /////////// BULLET COLLIDE //////////////
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Bullet"))
        {
            AdjustHealth(damage);
            Debug.Log("Bomber ded");
            GameManager.SendMessage("AddScore", SendMessageOptions.DontRequireReceiver);
            //GameObject.Destroy(gameObject);
        }
    }

    void AdjustHealth(float damage)
    {
        currenthealth -= damage;
        healthbarsize = currenthealth / maxhealth;
        HealthBar.transform.localScale = new Vector3(healthbarsize, 0.1f, 0);

        if (currenthealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }


}
