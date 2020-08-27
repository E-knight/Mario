using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    LevelManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        manager.AddCoin(transform.position + Vector3.down);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
