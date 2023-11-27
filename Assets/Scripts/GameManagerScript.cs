using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /////////////// TEXT /////////////////
    public GameObject player;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;

    public TextMeshProUGUI Text;
    string speedtext = "speed";
    string eventtext = "event";
    string weapon = "none";
    float playtime = 0;
    int Score = 0;
    float health;

    /////////////// AMMO /////////////////
    public float pistolammo;
    public float shotgunammo;
    public float minigunammo;
    public float railgunammo;
    public float grenadeammo;


    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //if (Instance == null) { Instance = this; } else if (Instance != this) { Destroy(this); }
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        pistolammo = 0;
        shotgunammo = 0;
        minigunammo = 0;
        railgunammo = 0;
        grenadeammo = 0;

    }

    private void Update()
    {
        playtime += 1.0F * Time.deltaTime;
        speedtext = player.GetComponent<PlayerMovement>().speedtext;
        eventtext = player.GetComponent<PlayerMovement>().eventtext;
        health = player.GetComponent<PlayerMovement>().currenthealth;
        weapon = player.GetComponent<PlayerMovement>().weapon;
        Textinfo();
        Showhealth(health);
    }

    void AddPistolAmmo()
    {
        pistolammo += 30;
    }

    void AddShotgunAmmo()
    {
        shotgunammo += 10;
    }

    void AddRailgunAmmo()
    {
        railgunammo += 10;
    }

    void AddGrenadeAmmo()
    {
        grenadeammo += 3;
    }

    void AddMinigunAmmo()
    {
        minigunammo += 30;
    }

    void ReducePistolAmmo()
    {
        pistolammo -= 1;
    }

    void ReduceShotgunAmmo()
    {
        shotgunammo -= 1;
    }

    void ReduceRailgunAmmo()
    {
        railgunammo -= 1;
    }

    void ReduceGrenadeAmmo()
    {
        grenadeammo -= 1;
    }

    void ReduceMinigunAmmo()
    {
        minigunammo -= 1;
    }

    void Textinfo()
    {
        float ammo = 0;
        if (weapon == "none") ammo = 0;
        if (weapon == "Pistol") ammo = pistolammo;
        if (weapon == "Shotgun") ammo = shotgunammo;
        if (weapon == "Minigun") ammo = minigunammo;
        if (weapon == "Grenade") ammo = grenadeammo;
        if (weapon == "Railgun") ammo = railgunammo;

        Text.text = "Speed: " + speedtext + "\n" + eventtext + "\n" + "Playtime: " + playtime + "\n" + "Score: " + Score + "\n" + "Weapon: " + weapon + "\n" + "Ammo: " + ammo;
    }

    void Showhealth(float health) 
    {
        //Debug.Log(health);

        
        //Vector3 pos = Camera.main.WorldToScreenPoint(0,1,0);
        //heart1.transform.position = pos;

        heart1.GetComponent<SpriteRenderer>().enabled = false;
        heart2.GetComponent<SpriteRenderer>().enabled = false;
        heart3.GetComponent<SpriteRenderer>().enabled = false;
        heart4.GetComponent<SpriteRenderer>().enabled = false;
        heart5.GetComponent<SpriteRenderer>().enabled = false;

        if (health > 0) heart1.GetComponent<SpriteRenderer>().enabled = true;
        if (health > 1) heart2.GetComponent<SpriteRenderer>().enabled = true;
        if (health > 2) heart3.GetComponent<SpriteRenderer>().enabled = true;
        if (health > 3) heart4.GetComponent<SpriteRenderer>().enabled = true;
        if (health > 4) heart5.GetComponent<SpriteRenderer>().enabled = true;


    }
}
