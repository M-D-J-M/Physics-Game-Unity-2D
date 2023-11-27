using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    // BULLET //
    //public GameObject player;
    public float timer;
    public float lifespan = 1;

    // GRENADE //
    public GameObject smallexplosion;
    ParticleSystem ps;
    public float radius = 6.0F;
    public float power = 1.8F;
    public string shooter = "Player";

    // SOUND //
    public GameObject boomsound;

    void Start()
    {

        timer += 1.0F * Time.deltaTime;
        if (timer >= lifespan)
        {
            GameObject.Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == (shooter) || other.gameObject.tag == ("Bullet"))
        {
            //do nothing
        }
        else
        {
            Debug.Log("Grenade BOOM");

            Vector2 explosionPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
            foreach (Collider2D hit in colliders)
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                float forceeffect = ((radius + 1) - distance) * power;

                if (rb != null)
                    rb.AddForce((hit.transform.position - transform.position) * forceeffect, ForceMode2D.Impulse);
            }

            ///////////// ANIMATION //////////////
            ///
            GameObject boomClone = Instantiate(smallexplosion);
            GameObject boomsoundClone = Instantiate(boomsound);

            // go to middle //
            boomClone.transform.position = transform.position;
            ps = boomClone.GetComponent<ParticleSystem>();

            ps.Play();
            
            GameObject.Destroy(gameObject);
        }
    }
}
