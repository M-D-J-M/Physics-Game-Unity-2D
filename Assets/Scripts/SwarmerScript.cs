using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmerScript : MonoBehaviour
{
    // ENEMY FOLLOW //
    public GameObject player;
    public GameObject GameManager;

    public float moveSpeed = 100.0f;
    Rigidbody2D rb;
    public float timer = 0.0f;
    public float maxtimer = 8.0f;
    public float attackRange = 15;


    //// HEALTH ////
    public GameObject HealthBar;
    public float maxhealth = 3;
    float currenthealth;
    float damage = 1;
    float healthbarsize;

    ////////////// spawn small swarmer ///////////////
    public GameObject smallswarmer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        currenthealth = maxhealth;
    }

    void Update()
    {
        // PLAYER DISTANCE //
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // Rotate //
        if (distance < attackRange) { Rotatetotarget(); }

        // TIMER //
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            //// ATTACK RANGE ////
            if (distance < attackRange) { Attack(); }
            timer = maxtimer;
        }
        //Rotatetotarget();
    }

    void Rotatetotarget()
    {
        float maxAngle = 10f;

        // where is the target?
        Vector2 targetDirection = player.transform.position - transform.position;
        // where are we looking?
        Vector2 lookDirection = -transform.up;

        // to indicate the sign of the (otherwise positive 0 .. 180 deg) angle
        Vector3 cross = Vector3.Cross(targetDirection, lookDirection);
        // actually get the sign (either 1 or -1)
        float sign = Mathf.Sign(cross.z);

        // the angle, ranging from 0 to 180 degrees
        float angle = Vector2.Angle(targetDirection, lookDirection);

        // apply the sign to get angles ranging from -180 to 0 to +180 degrees
        angle *= sign;
        // debug output
        //Text.text = angle.ToString();
        float turnforce = sign / 3;

        // apply torque in the opposite direction to decrease angle
        if (Mathf.Abs(angle) > maxAngle) GetComponent<Rigidbody2D>().AddTorque(-turnforce);

    }

    private void Attack()
    {
        rb.AddForce((player.transform.position - transform.position) * moveSpeed);
    }

    /////////// BULLET COLLIDE //////////////
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Bullet"))
        {
            AdjustHealth(damage);
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
            GameObject smallswarmer1 = Instantiate(smallswarmer, transform.position, Quaternion.identity);

            // Position of bullet to origin firepoint //
            smallswarmer1.transform.position = transform.position + new Vector3 (1.0f, 0.0f, 0.0f);

            // Spawn the bullet from the prefab.
            GameObject smallswarmer2 = Instantiate(smallswarmer, transform.position, Quaternion.identity);

            // Position of bullet to origin firepoint //
            smallswarmer2.transform.position = transform.position + new Vector3(-1.0f, 0.0f, 0.0f);

            GameObject.Destroy(gameObject);
        }
    }
}
