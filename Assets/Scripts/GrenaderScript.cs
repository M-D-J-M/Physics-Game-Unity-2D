using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenaderScript : MonoBehaviour
{

    ////////////// PEW PEW ///////////////
    public GameObject coin;
    public GameObject grenade;
    public GameObject GameManager;
    public Transform firePoint;
    public GameObject player;
    public float bulletSpeed = 15.0f;
    float bulletdelay = 3;
    public float attackRange = 15;

    //// HEALTH ////
    public GameObject HealthBar;
    public float maxhealth = 3;
    float currenthealth;
    float damage = 1;
    float healthbarsize;


    void Start()
    {
        currenthealth = maxhealth;
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
        player = GameObject.FindGameObjectWithTag("Player");
        //Grenade(1,0);
        
    }

    void Update()
    {
        Grenade(1, 0);
        
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
            coin1.transform.position = transform.position + new Vector3(0.7f, 0.0f, 0.0f);

            // Spawn the bullet from the prefab.
            GameObject coin2 = Instantiate(coin, transform.position, Quaternion.identity);

            // Position of bullet to origin firepoint //
            coin2.transform.position = transform.position + new Vector3(-0.7f, 0.0f, 0.0f);

            GameObject.Destroy(gameObject);
        }
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

    void Grenade(float number, float spread)
    {

        Vector3 pos = transform.position;
        Vector3 dir = player.transform.position - pos;
        float distance = Vector3.Distance(player.transform.position, transform.position);

        // TIMER FOR BULLETS //
        bulletdelay -= 1.0F * Time.deltaTime;

        //Debug.Log(bulletdelay);
        if (distance < attackRange)
        {
            if (bulletdelay < 0)

            {
                for (float i = 0; i < number; i++)
                {
                    // point forward
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    float startangle = spread * 3;
                    float fireangle = startangle - (spread * i); // spread 20
                    float firespread = angle + fireangle / 2;

                    firePoint.rotation = Quaternion.AngleAxis(firespread, Vector3.forward);

                    // make new bullet //
                    GameObject bulletClone = Instantiate(grenade);

                    // make launcher of grenade imune to collisions

                    bulletClone.GetComponent<GrenadeScript>().shooter = "Grenader";

                    // go to middle //
                    bulletClone.transform.position = firePoint.position;
                    // rotate shape towards mouse //
                    bulletClone.transform.rotation = Quaternion.Euler(0, 0, angle);
                    float start = 0;

                    if (number > 1)
                    {
                        start = 1;
                    }

                    float spacing = -3 / number;
                    //Debug.Log("spacing:" + start);

                    float position = start + (spacing * i); // 5)2,1,0,-1,-2  3)1,0,-1

                    bulletClone.transform.Translate(0, position, 0);
                    bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * bulletSpeed;

                    // Recoil //
                    //Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    //rb.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * -bulletforce, ForceMode2D.Impulse);
                    bulletdelay = 3;
                }
            }
        }
    }
}
