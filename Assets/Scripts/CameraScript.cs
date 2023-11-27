using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    /////////// VELOCITY ///////////////
    public GameObject player;
    public Vector2 offset;
    float velocityx;
    float velocityy;
    float velocity;
    public Camera cam;

    ///////////// OFFSET //////////
    float offsetx;
    float offsety;
    //float startoffsetx = 0f;
    float startoffsety = 2f;
    public float maxoffset = 2f;
    public float offsetchange = 0.005f;
    public float beginoffset = 6f;


    ///////////// SPEED //////////
    float screenchange = 0.01f;
    float minv = 8f;
    float maxv = 50f;

    ///////////// RANGE //////////
    public float nearzoom = 10f;
    public float zoomrange = 4;
    public float minnearzoom = 1f;
    public float maxnearzoom = 20f;
    public float farzoom;
    public float targetzoom;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //farzoom = nearzoom + zoomrange;
    }

    private void Update()
    {
        farzoom = nearzoom + 4;
        /////////////// VELOCITY ////////////////
        
        velocityx = player.GetComponent<Rigidbody2D>().velocity.x;
        velocityy = player.GetComponent<Rigidbody2D>().velocity.y;
        velocity = Mathf.Sqrt(Mathf.Pow(velocityx, 2) + Mathf.Pow(velocityy, 2));

        //////////////// OFFSET /////////////////

        float offsetlerpx = Mathf.InverseLerp(minv, maxv, velocityx); 
        float offsetlerpy = Mathf.InverseLerp(minv, maxv, velocityy);
        float offsetrangex = offsetlerpx * maxoffset;
        float offsetrangey = offsetlerpy * maxoffset;

        ///// OFFSET X //////
        
        // move fast x //
        if (velocityx > beginoffset) // moving right
        {
            offsetx += offsetchange;
            if (offsetx > maxoffset) 
            {
                offsetx = maxoffset;
            }
        }

        if (velocityx < -beginoffset) // moving left
        {
            offsetx -= offsetchange;
            if (offsetx < -maxoffset)
            {
                offsetx = -maxoffset;
            }
        }
        // move slow y , move back to center //
        if (velocityx >= -beginoffset && velocityx <= beginoffset) 
        { 
            if (offsetx > 0) 
            { 
                offsetx -= offsetchange;

            }

            if (offsetx < 0)
            {
                offsetx += offsetchange;

            }

        }

        ///// OFFSET Y //////

        // move fast y //
        if (velocityy > beginoffset) // moving up
        {
            offsety += offsetchange;
            if (offsety > maxoffset)
            {
                offsety = maxoffset;
            }
        }

        if (velocityy < -beginoffset) // moving down
        {
            offsety -= offsetchange;
            if (offsety < -maxoffset)
            {
                offsety = -maxoffset;
            }
        }
        // move slow y , move back to center //
        if (velocityy >= -beginoffset && velocityy <= beginoffset)
        {
            if (offsety > 0)
            {
                offsety -= offsetchange;

            }

            if (offsety < 0)
            {
                offsety += offsetchange;

            }

        }

        //Debug.Log(offsetx);
  

        ///////// CAMERA FOLLOWS PLAYER /////////

        transform.position = new Vector2(player.transform.position.x + offsetx, player.transform.position.y + offsety + startoffsety);

        ///////////////// ZOOM //////////////////
        
        float zoomlerp = Mathf.InverseLerp(minv, maxv, velocity);
        float zoomrange = zoomlerp * (farzoom-nearzoom);
        targetzoom = nearzoom + zoomrange;

        if (cam.orthographicSize < targetzoom)
        {
            cam.orthographicSize += screenchange;
        }
        if (cam.orthographicSize > targetzoom)
        {
            cam.orthographicSize -= screenchange;
        }



        if (Input.GetKeyUp(KeyCode.UpArrow)) 
        {
            if (nearzoom < maxnearzoom) nearzoom -= 1;

        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (nearzoom > minnearzoom) nearzoom += 1;
        }

        //Debug.Log(targetzoom);




    }
}
