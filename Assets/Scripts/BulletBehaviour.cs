using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public GameObject player;
    public float timer;
    public float lifespan = 1;
    string weapon;
    public float shotgunlifespan = 0.5f;
    public TrailRenderer TrailRenderer;

    void Start()
    {
        TrailRenderer = GetComponent<TrailRenderer>();
        TrailRenderer.enabled = true;
        player = GameObject.FindGameObjectWithTag("Player");
        weapon = player.GetComponent<PlayerMovement>().weapon;
        if (weapon == "Shotgun") 
        {
            lifespan = shotgunlifespan;
        }
    }

    void Update()
    {
        timer += 1.0F * Time.deltaTime;
        if (timer >= lifespan)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player") || other.gameObject.tag == ("Bullet"))
        {
            //do nothing
        }
        else 
        {
            GameObject.Destroy(gameObject);
        }
    }
}
