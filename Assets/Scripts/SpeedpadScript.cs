using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SpeedpadScript : MonoBehaviour
{
    public bool speedpad = false;
    public float speedpadboost = 50f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Lava") || collision.gameObject.tag == ("Floor") || collision.gameObject.tag == ("Lava"))
        {
            // do nafin
        }
        else
        {
            //Debug.Log("speedpad");
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            //Debug.Log(collision);
            rb.AddForce(transform.right * speedpadboost, ForceMode2D.Force);
        }
    }

}
