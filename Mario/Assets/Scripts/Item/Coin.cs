using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
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
        if(collision.gameObject.tag=="Player")
        {
            Debug.Log("金币" + Time.time.ToString());
            manager.AddCoin();
            Destroy(gameObject);
            Debug.Log("金币" + Time.time.ToString());
        }
    }
}
