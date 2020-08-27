using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public string scenename;
    LevelManager manager;
    bool canmove;
    Transform flag, flagpos;
    float movespeed = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        flag = transform.Find("castle_flag");
        flagpos = transform.Find("flag_pos");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(canmove)
        {
            if (flag.position.y < flagpos.position.y)
                flag.position = new Vector2(flag.position.x, flag.position.y + movespeed);
            else
                manager.LoadNewLevel(scenename);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            canmove = true;
            manager.FinishLevel();
        }
    }
}
