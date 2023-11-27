using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    public GameObject follow;
    public float offset = -0.2f;
    void Start()
    {
        //follow = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = new Vector3 (follow.transform.position.x, follow.transform.position.y + offset, follow.transform.position.z);
    }
}
