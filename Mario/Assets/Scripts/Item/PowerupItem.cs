using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupItem : MonoBehaviour
{
    LevelManager manager;
    Rigidbody2D rb;
    public Vector2 v;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        manager.soundsource.PlayOneShot(manager.Powerupsound);
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = v;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            /*for(int i=0;i<60;i++)
            {
                Debug.Log("1111");
            }*/
            /*manager.MarioGetSmaller();
            Destroy(gameObject);*/
            manager.MarioGetBig();
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
