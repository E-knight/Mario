using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    LevelManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            manager.MarioDie();
        else
            Destroy(collision.gameObject);
    }
}
