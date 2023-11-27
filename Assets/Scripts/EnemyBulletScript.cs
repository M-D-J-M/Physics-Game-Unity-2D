using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public float timer;
    public float lifespan = 1;
    public float shotgunlifespan = 0.5f;
    public TrailRenderer TrailRenderer;

    void Start()
    {
        TrailRenderer = GetComponent<TrailRenderer>();
        TrailRenderer.enabled = true;
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
        if (other.gameObject.tag == ("Enemy") || other.gameObject.tag == ("Enemybullet"))
        {
            //do nothing
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}
