using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Starman : MonoBehaviour
{
    LevelManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
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
            manager.Mariostarman();
            manager.soundsource.PlayOneShot(manager.Powerupsound);
            Destroy(gameObject);        }
    }
}
