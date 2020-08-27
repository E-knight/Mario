using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    LevelManager manager;
    Transform flag, stop_pos;
    bool canmove;
    float movespeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        flag = transform.Find("goal_flag_green");
        stop_pos = transform.Find("stop_position");
    }

    private void FixedUpdate()
    {
        if (canmove && flag.position.y > stop_pos.position.y)
            flag.position = new Vector2(flag.position.x, flag.position.y - movespeed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player"&&!canmove)
        {
            canmove = true;
            Debug.Log("马里奥爬旗杆");
            manager.MarioFlag();
        }
    }
}
