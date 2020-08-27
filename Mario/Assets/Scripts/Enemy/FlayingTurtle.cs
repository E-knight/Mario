using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlayingTurtle : Enemy
{
    public GameObject turtle;
    // Start is called before the first frame update
    void Start()
    {
        starmanscore = 100;
        trutleshellscore = 500;
        dropscore = 100;
        firehitscore = 100;
        trampscore = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void FlipDie()
    {
        base.FlipDie();
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
    //变成普通乌龟协程
    IEnumerator LoseWingCoroutine()
    {
        StopMove();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log("生成乌龟");
        GameObject Turtle = Instantiate(turtle, transform.position, Quaternion.identity);
        Turtle.GetComponent<Move>().direction = transform.localScale.x;
        Destroy(gameObject.transform.parent.gameObject);
        istramped = false;
    }
    void LoseWing()
    {
        StartCoroutine(LoseWingCoroutine());
    }
    public override void TurtleShell()
    {
        FlipDie();
    }
    public override void Drop()
    {
        LoseWing();
    }
    public override void Fireball()
    {
        FlipDie();
    }
    public override void Tramp()
    {
        istramped = true;
        LoseWing();
    }
}
