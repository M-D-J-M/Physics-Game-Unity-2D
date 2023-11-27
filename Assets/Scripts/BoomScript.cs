using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomScript : MonoBehaviour
{

    public float timer = 0;
    public float lifespan = 1;


    void Start()
    {
        
    }


    void Update()
    {
        timer += 1.0F * Time.deltaTime;
        if (timer >= lifespan)
        {
            GameObject.Destroy(gameObject);
        }

    }
}
