using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{
   float disappear_time;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        disappear_time = 0.5f;
        starmanscore = 100;
        trutleshellscore = 500;
        dropscore = 100;
        firehitscore = 100;
        trampscore = 100;
    }

    // Update is called once per frame
    public override void Tramp()
    {
        Debug.Log("被踩");
        istramped = true;
        StopMove();
       
        anim.SetTrigger("tramp");
        Destroy(gameObject, disappear_time);
        istramped = false;
    }
}
