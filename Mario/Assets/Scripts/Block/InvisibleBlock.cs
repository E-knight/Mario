using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleBlock : MonoBehaviour
{
    BoxCollider2D boxCollider;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxCollider.enabled = false;
        sprite.enabled = false;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            boxCollider.enabled = true;
            sprite.enabled = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
