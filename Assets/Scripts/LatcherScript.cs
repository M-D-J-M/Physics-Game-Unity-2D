using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LatcherScript : MonoBehaviour
{
    /////////// GRAPPLING HOOK ///////////
    public FixedJoint2D joint;
    public GameObject player;
    public Rigidbody2D rb;
    bool starttimer = false;
    public float timer;
    public float lifespan = 5;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (starttimer) 
        {
            timer += 1.0F * Time.deltaTime;
            if (timer >= lifespan)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Player"))
        {
            Debug.Log("sticky");
            joint.connectedBody = rb;
            starttimer = true;
        }
    }
}
