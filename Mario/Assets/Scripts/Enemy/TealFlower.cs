using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TealFlower : Enemy
{
    LevelManager manager;
    GameObject mario;
    MoveVertical move;
    CircleCollider2D collider;
    bool cansee;
    float movedistance=2;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        mario = FindObjectOfType<Mario>().gameObject;
        move = GetComponent<MoveVertical>();
        collider = GetComponent<CircleCollider2D>();
        cansee = false;
        move.canmove = false;
        collider.enabled = false;
        starmanscore = 100;
        trutleshellscore = 500;
        dropscore = 0;
        firehitscore = 200;
        trampscore = 0;
    }

    private void OnBecameVisible()
    {
        cansee = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(cansee)
        {
            if(Mathf.Abs(mario.transform.position.x-transform.position.x)>movedistance)
            {
                collider.enabled = true;
                move.canmove = true;
            }
            else if(move.atdown)
            {
                collider.enabled = false;
                move.canmove = false;
            }
        }
    }
    void DestroyFlower()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
    public override void Starman()
    {
        DestroyFlower();
    }
    public override void TurtleShell()
    {
        DestroyFlower();
    }
    public override void Drop()
    {
    }
    public override void Fireball()
    {
        DestroyFlower();
    }
    public override void Tramp()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            manager.MarioGetSmaller();
    }
}
