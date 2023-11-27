using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedbarrelScript : MonoBehaviour
{
    ParticleSystem ps;
    public float radius = 6.0F;
    public float power = 1.8F;

    //////////////// AUDIO ///////////////
    public AudioClip explosion;
    public GameObject boomsound;

    // Start is called before the first frame update
    void Start()
    {
        ps = GameObject.Find("Explosion").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Bullet"))
        {
            Vector2 explosionPos = transform.position;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPos, radius);
            foreach (Collider2D hit in colliders)
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                float distance = Vector2.Distance (transform.position, hit.transform.position);
                float forceeffect = ((radius+1) - distance) * power;

                if (rb != null)
                rb.AddForce((hit.transform.position - transform.position) * forceeffect, ForceMode2D.Impulse);
            }

            ///////////// ANIMATION //////////////

            GameObject boomsoundclone = Instantiate(boomsound);
            ps.Play();
            Debug.Log("BOOM");
            GameObject.Destroy(gameObject);

        }
    }
}
