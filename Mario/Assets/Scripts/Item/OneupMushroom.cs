using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneupMushroom : MonoBehaviour
{
    public Vector2 speed;
    LevelManager manager;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed;
        manager.soundsource.PlayOneShot(manager.Powerappearsound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            manager.AddLife(gameObject.transform.position + Vector3.up * 2);
            Destroy(gameObject);
        }
    }
}
