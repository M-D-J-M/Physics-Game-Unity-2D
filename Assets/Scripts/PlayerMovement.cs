using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    /////////////// TEXT /////////////////
    public string speedtext = "speed";
    public string eventtext = "event";

    ////////////// RESPAWN ///////////////
    public bool dead;
    public Vector2 checkpoint = new Vector2(0, 1);

    ////////////// PEW PEW ///////////////
    public GameObject bullet;
    public GameObject grenade;
    public Transform firePoint;
    public float bulletSpeed = 15.0f;
    float bulletforce = 2;
    public string weapon = "none";
    public float firerate = 0.0f;

    ////////////// MOVEMENT //////////////
    Rigidbody2D rb;
    public float rotationSpeed = 5.0f;
    public float jumpSpeed = 5.0f;
    public float maxSpeed = 2000.0f;

    ////////////// JUMPING ///////////////
    public bool grounded;
    public float superjump = 4.0f;

    ////////////// PADS ///////////////
    public bool sgrounded;
    public bool speedpad;
    public float speedpadboost = 7f;

    /////////// GRAPPLING HOOK ///////////
    public Camera mainCamera;
    public LineRenderer _lineRenderer;
    public DistanceJoint2D _distanceJoint;

    /////////// GRAPPLING HOOK ///////////
    public GameObject GameManager;

    /////////////// HEALTH ///////////////
    public float maxhealth = 5;
    public float currenthealth;

    //////////////// AUDIO ///////////////
    public AudioClip jump;
    public AudioClip shoot;
    public AudioClip hurt;
    public AudioClip powerup;
    public AudioClip coin;
    AudioSource audioSource;

    //////////////// DAMAGE/SPRITE ///////////////
    SpriteRenderer sprite;
    float invunerabletimer;
    public float invunerablemaxtime = 2.0f;
    public float superinvunerablemaxtime = 10.0f;
    bool gothurt = false;

    //////////////// BOUNCE ///////////////
    bool bounce = false;
    CircleCollider2D myCC2D;

    //////////////// AMMO ///////////////
    float pistolammo = 0;
    float shotgunammo = 0;
    float minigunammo = 0;
    float railgunammo = 0;
    float grenadeammo = 0;

    void Start()
    {
        // bounceyness
        myCC2D = GetComponent<CircleCollider2D>();

        // Set max invunerable time //
        invunerabletimer = invunerablemaxtime;

        // Sprite //
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(0, 1, 0, 1);

        // audio //
        audioSource = GetComponent<AudioSource>();
        
        // instance of rb movement //
        rb = GetComponent<Rigidbody2D>();

        // set the joint off as not selected yet //
        _distanceJoint.enabled = false;

        // instance gamemanger //
        GameManager = GameObject.FindGameObjectWithTag("GameManager");

        // set health //
        currenthealth = maxhealth;



    }

    void Update()
    {
       UpdateAmmo();
       Textinfo();
       Shooting();
       Spinning();
       Jumping();
       Returntocheckpoint();
       Grapplinghook();
       Shortcuts();
       Speedpad();
       AdjustColour();
       Bounce();
       

       firerate -= Time.deltaTime;
       //Debug.Log(firerate);



        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == ("Ground") )
        {
            eventtext = "Grounded";
            grounded = true;
        }

        if (collision.gameObject.tag == ("Jumppad"))
        {
            eventtext = "SGrounded";
            sgrounded = true;
        }

        if (collision.gameObject.tag == ("Speedpad"))
        {
            eventtext = "Speedpad";
            grounded = true;
            speedpad = true;
        }

        else if (collision.gameObject.tag == ("Lava"))
        {
            eventtext = "Lava x.x";
            audioSource.PlayOneShot(hurt, 0.7F);
            Respawn();
        }

        else if (collision.gameObject.tag == ("Pistol"))
        {
            weapon = "Pistol";
            audioSource.PlayOneShot(powerup, 0.7F);
            GameManager.SendMessage("AddPistolAmmo", SendMessageOptions.DontRequireReceiver);
            GameObject.Destroy(collision.gameObject);
            
        }

        else if (collision.gameObject.tag == ("Shotgun"))
        {
            weapon = "Shotgun";
            GameManager.SendMessage("AddShotgunAmmo", SendMessageOptions.DontRequireReceiver);
            audioSource.PlayOneShot(powerup, 0.7F);
            GameObject.Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == ("Minigun"))
        {
            weapon = "Minigun";
            GameManager.SendMessage("AddMinigunAmmo", SendMessageOptions.DontRequireReceiver);
            audioSource.PlayOneShot(powerup, 0.7F);
            GameObject.Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == ("Railgun"))
        {
            weapon = "Railgun";
            GameManager.SendMessage("AddRailgunAmmo", SendMessageOptions.DontRequireReceiver);
            audioSource.PlayOneShot(powerup, 0.7F);
            GameObject.Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == ("Grenade"))
        {
            weapon = "Grenade";
            GameManager.SendMessage("AddGrenadeAmmo", SendMessageOptions.DontRequireReceiver);
            audioSource.PlayOneShot(powerup, 0.7F);
            GameObject.Destroy(collision.gameObject);
        }

        else if (collision.gameObject.tag == ("InvunerableBox"))
        {
            eventtext = "Speedpad";
            audioSource.PlayOneShot(powerup, 0.7F);
            GameObject.Destroy(collision.gameObject);
            invunerabletimer = superinvunerablemaxtime;
            gothurt = true;
        }

        else if (collision.gameObject.tag == ("Coin"))
        {
            GameObject.Destroy(collision.gameObject);
            audioSource.PlayOneShot(coin, 0.7F);
            GameManager.SendMessage("AddScore", SendMessageOptions.DontRequireReceiver);
        }

        else if (collision.gameObject.tag == ("Enemy") || (collision.gameObject.tag == ("Enemybullet")))
        {
            if (gothurt == false){ AdjustHealth(1); }
        }

        else
        {
           // grounded = false;
            dead = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == ("Ground"))
        {
            //eventtext = "Grounded";
            grounded = true;
        }
    }

        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Enemybullet"))
        {
            AdjustHealth(1);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            grounded = false;
        }

        if (collision.gameObject.tag == ("Jumppad"))
        {

            sgrounded = false;

        }

        if (collision.gameObject.tag == ("Speedpad"))
        {
            grounded = false;
            speedpad = false;
        }


    }

    void Textinfo()
    {
        speedtext = rb.velocity.ToString();
        
    }

    void UpdateAmmo()
    {
        pistolammo = GameManager.GetComponent<GameManager>().pistolammo;
        shotgunammo = GameManager.GetComponent<GameManager>().shotgunammo;
        minigunammo = GameManager.GetComponent<GameManager>().minigunammo;
        grenadeammo = GameManager.GetComponent<GameManager>().grenadeammo;
        railgunammo = GameManager.GetComponent<GameManager>().railgunammo;
    }

    void Shooting()
    {

        if (weapon == "none")
        {
            //none
        }

        if (weapon == "Pistol")
        {
            if (pistolammo > 0) GunSystem(1, 0, 20, 1f, 0, 0, bullet);
        }

        if (weapon == "Minigun")
        {
            if (minigunammo > 0) GunSystem(1, 0, 30, 0.2f, 0, 0, bullet);
        }

        if (weapon == "Shotgun")
        {
            if (shotgunammo > 0) GunSystem(7, 90, 20, 1, 0, 2, bullet);
        }

        if (weapon == "Railgun")
        {
            if (railgunammo > 0) GunSystem(3, 0, 30, 2f, 2, 1, bullet);
        }

        if (weapon == "Grenade")
        {
            if (grenadeammo > 0) GunSystem(1, 0, 10, 2, 0, 0, grenade);
        }


    }

    void ReduceAmmo() 
    {

        if (weapon == "Pistol")
        {
            GameManager.SendMessage("ReducePistolAmmo", SendMessageOptions.DontRequireReceiver);
        }

        if (weapon == "Minigun")
        {
            GameManager.SendMessage("ReduceMinigunAmmo", SendMessageOptions.DontRequireReceiver);
        }

        if (weapon == "Shotgun")
        {
            GameManager.SendMessage("ReduceShotgunAmmo", SendMessageOptions.DontRequireReceiver);
        }

        if (weapon == "Railgun")
        {
            GameManager.SendMessage("ReduceRailgunAmmo", SendMessageOptions.DontRequireReceiver);
        }

        if (weapon == "Grenade")
        {
            GameManager.SendMessage("ReduceGrenadeAmmo", SendMessageOptions.DontRequireReceiver);
        }
    }

    void GunSystem(float bulletnumber, float anglespread, float speed, float bulletlag, float spacing, float recoil, GameObject projectile)
    {
        // I guess position of player
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        // Direction from player to mouse
        Vector3 dir = Input.mousePosition - pos;

        // Figure out angle to mouse //
        float zeroangle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Calculate anglegap between bullets depending on spread and number bullets, avoid divide by 0
        float bulletanglegap = 0;
        if (bulletnumber > 1) {bulletanglegap = anglespread / (bulletnumber - 1); }

        // Calculate spreadgap between bullets depending on spacing and number bullets, avoid divide by 0
        float bulletspreadgap = 0;
        if (bulletnumber > 1) { bulletspreadgap = spacing / (bulletnumber - 1); }

        // Set firepoint rotation to the angle to mouse.
        firePoint.rotation = Quaternion.AngleAxis(zeroangle, Vector3.forward);

        // if firerate is below zero then ready to shoot
        if (firerate < 0)
        {
            // If fire button pressed
            if (Input.GetMouseButton(0))
            {

                ReduceAmmo();
                // PLay sound effect of shot
                audioSource.PlayOneShot(shoot, 0.7F);

                // Add recoil //
                Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                rb.AddForce((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized * -bulletforce, ForceMode2D.Impulse);

                // For each bullet do this
                for (float i = 0; i < bulletnumber; i++)
                {
                    // Find start angle, for 90 spread , -45 degrees start
                    float startangle = zeroangle - (anglespread / 2);

                    // Adjust angle based on spread and number, for 7(-45,-30,-15,0,15,30,45)
                    float fireangle = startangle + (bulletanglegap * i);

                    // Reset firepoint rotation to desired angle.
                    firePoint.rotation = Quaternion.AngleAxis(fireangle, Vector3.forward);

                    // Spawn the bullet from the prefab.
                    GameObject bulletClone = Instantiate(projectile, firePoint.position, Quaternion.identity);

                    // Position of bullet to origin firepoint //
                    bulletClone.transform.position = firePoint.position;

                    // rotate bullet towards mouse, not that that really matters //
                    bulletClone.transform.rotation = Quaternion.Euler(0, 0, fireangle);

                    // Adjust bulletclone position depending on spacing
                    float positionadjust = (-spacing / 2) + (bulletspreadgap * i);

                    //float positionadjust = 2;
                    bulletClone.transform.Translate(0, positionadjust, 0);

                    // fire bullet
                    bulletClone.GetComponent<Rigidbody2D>().velocity = firePoint.right * speed;

                    // Reset firerate.
                    firerate = bulletlag;
                }
            }
        }
    }

    void Spinning() 
    {
        if (Input.GetKey(KeyCode.D))
        {
            eventtext = "Right Spin";
            rb.AddTorque(-rotationSpeed);
            if (rb.angularVelocity < (-maxSpeed)) { rb.angularVelocity = -maxSpeed; }
        }

        if (Input.GetKey(KeyCode.A))
        {
            eventtext = "Left Spin";
            rb.AddTorque(rotationSpeed);
            if (rb.angularVelocity > maxSpeed) { rb.angularVelocity = maxSpeed; }
        }

    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

            if (grounded && sgrounded)
            {
                eventtext = "Jump";
                rb.AddForce(new Vector2(0, superjump) * jumpSpeed, ForceMode2D.Impulse);
                grounded = false;
                audioSource.PlayOneShot(jump, 0.7F);
            }

            else if (sgrounded && !grounded)
            {
                eventtext = "Super Jump";
                rb.AddForce(new Vector2(0, superjump) * jumpSpeed, ForceMode2D.Impulse);
                sgrounded = false;
                audioSource.PlayOneShot(jump, 0.7F);
            }

            else if (grounded)
            {
                eventtext = "Super Jump";
                rb.AddForce(new Vector2(0, 2) * jumpSpeed, ForceMode2D.Impulse);
                sgrounded = false;
                audioSource.PlayOneShot(jump, 0.7F);
            }

            else
            {
                eventtext = "No Jump";
            }


        }
    }

    void Bounce()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            bounce = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            bounce = false;
        }

        if (bounce) 
        {
            PhysicsMaterial2D physicsMaterial2D = myCC2D.sharedMaterial;
            physicsMaterial2D.bounciness = 0.9f;
            myCC2D.sharedMaterial = physicsMaterial2D;


        }

        if (!bounce)
        {
            PhysicsMaterial2D physicsMaterial2D = myCC2D.sharedMaterial;
            physicsMaterial2D.bounciness = 0;
            myCC2D.sharedMaterial = physicsMaterial2D;

        }
        //Debug.Log(myCC2D.sharedMaterial.bounciness);
    }

    void Speedpad() 
    {
        if (speedpad)
        {
            //eventtext = "Speedpad";
            //rb.AddForce(new Vector2(1, 0) * speedpadboost, ForceMode2D.Force);
        }
    }

    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Returntocheckpoint()
    {
        /////// RETURN TO CHECKPOINT YO ///////

        if (Input.GetKeyDown(KeyCode.R))
        {
            eventtext = "Return to Checkpoint";
            Respawn();
        }

    }

    void Grapplinghook() 
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            eventtext = "Grappling Hook Made";
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.SetPosition(0, mousePos);
            _lineRenderer.SetPosition(1, transform.position);
            _distanceJoint.connectedAnchor = mousePos;
            _distanceJoint.enabled = true;
            _lineRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            eventtext = "Grappling Hook Released";
            _distanceJoint.enabled = false;
            _lineRenderer.enabled = false;
        }
        if (_distanceJoint.enabled)
        {
            _lineRenderer.SetPosition(1, transform.position);
        }
    }

    void Shortcuts() 
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = "Pistol";
            eventtext = "Pistol";
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = "Minigun";
            eventtext = "Minigun";
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = "Shotgun";
            eventtext = "Shotgun";
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weapon = "Railgun";
            eventtext = "Railgun";
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            weapon = "Grenade";
            eventtext = "Grenade";
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            weapon = "none";
            eventtext = "no weapon";
        }
    }

    void AdjustHealth(float damage)
    {
        audioSource.PlayOneShot(hurt, 0.7F);
        
        currenthealth -= damage;
        if (currenthealth <= 0)
        {
            //GameObject.Destroy(gameObject);
            Respawn();

        }
        gothurt = true;
        //chnage colour for hurt
        //sprite.color = new Color(1, 1, 1, 0);
    }

    void AdjustColour() 
    {
        //Debug.Log(invunerabletimer);
        if (gothurt) 
        {
            
            invunerabletimer  -= Time.deltaTime;
            sprite.color = new Color(1, 0, 1, 1); //magenta

        }
        if (invunerabletimer <= 0) 
        {
            invunerabletimer = invunerablemaxtime;
            gothurt = false;
            sprite.color = new Color(0, 1, 0, 1); // green

        }
    }
}
