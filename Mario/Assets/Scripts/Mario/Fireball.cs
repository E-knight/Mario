using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float direction;
    float explosiontime = 0.3f;
    Vector2 absspeed = new Vector2(22, 12);
    LevelManager manager;
    Rigidbody2D rb;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction * absspeed.x, -absspeed.y);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(direction * absspeed.x, rb.velocity.y);
    }
    void Explode()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        anim.SetTrigger("hit");
        manager.soundsource.PlayOneShot(manager.Bumpsound);
        Destroy(gameObject, explosiontime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            manager.FireballEnemy(enemy);
            Explode();
        }
        else
        {
            Vector2 normal = collision.contacts[0].normal;
            Vector2 leftside = new Vector2(-1, 0);
            Vector2 rightside = new Vector2(1, 0);
            Vector2 bottomside = new Vector2(0, 1);
            if (normal == leftside || normal == rightside)
                Explode();
            else if (normal == bottomside)
                rb.velocity = new Vector2(rb.velocity.x, absspeed.y);
            else
                rb.velocity = new Vector2(rb.velocity.x, -absspeed.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            manager.FireballEnemy(enemy);
            Explode();
        }
    }
}
