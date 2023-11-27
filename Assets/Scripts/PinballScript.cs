using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinballScript : MonoBehaviour
{
    Rigidbody rb;
    public float power = 20F;

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        Debug.Log("boing");
        //Rigidbody2D rb = collision2D.GetComponent<Rigidbody2>();
        collision2D.rigidbody.AddForce((collision2D.transform.position - transform.position) * power, ForceMode2D.Impulse);
    }
}
