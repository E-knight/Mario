using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Enemy
{
    public GameObject turtleshell;
    // Start is called before the first frame update
    void Start()
    {
        starmanscore = 200;
        trutleshellscore = 500;
        dropscore = 100;
        firehitscore = 200;
        trampscore = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //被踩踏协程
    IEnumerator  GenerateTurtleShell()
    {
        StopMove();
        gameObject.GetComponent<SpriteRenderer>().enabled=false;
        yield return new WaitForSecondsRealtime(0.1f);
        Instantiate(turtleshell, transform.position, Quaternion.identity);
        /*TrutleShell shell = FindObjectOfType<TrutleShell>();
        shell.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;*/
        Destroy(gameObject);
        istramped = false;

    }
    //重写被踩踏方法
    public override void Tramp()
    {
        istramped = true;
        StartCoroutine(GenerateTurtleShell());
    }
}
