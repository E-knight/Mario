using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrutleShell : Enemy
{
    LevelManager manager;
    Rigidbody2D rb;
    Animator anim;
    Mario mario;
    public GameObject Turtle;
    public float rollspeed = 7;
    public float waittillrevive = 5;
    public float waittillrespawn = 1.5f;
    public float shelltime;
    float currentrollspeed;
    bool isrevive;
    public bool isroll;
    bool trampbefore = false;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        rb = GetComponent<Rigidbody2D>();
        mario = FindObjectOfType<Mario>();
        anim = GetComponent<Animator>();
        isrevive = false;
        isroll=false;
        starmanscore = 200;
        trutleshellscore = 500;
        dropscore = 100;
        firehitscore = 100;
        trampscore = 500;
        istramped = true;
        shelltime = Time.time;
        Debug.Log("生成乌龟壳");
    }

    // Update is called once per frame
    void Update()
    {
        if(!isroll&&!isrevive)
        {
            waittillrevive -= Time.deltaTime;
            if (waittillrevive <= 0)
            {
                anim.SetTrigger("revive");
                isrevive = true;
            }
            

        }
else if (isrevive && !isroll)
            {
                waittillrespawn -= Time.deltaTime;
                if (waittillrespawn <= 0)
                {
                    Instantiate(Turtle, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        else if (isroll)
                rb.velocity = new Vector2(currentrollspeed, rb.velocity.y);
        if (trampbefore)
               trampscore = 0;

    }

    public override void Tramp()
    {
        Debug.Log("乌龟壳");
        istramped = true;
       /* if (!isroll)
        {
            if (mario.transform.localScale.x == 1)
                currentrollspeed = rollspeed;
            else if (mario.transform.position.x == -1)
                currentrollspeed = -rollspeed;
            rb.velocity = new Vector2(currentrollspeed, rb.velocity.y);
            isroll = true;
            anim.SetTrigger("roll");
        }
        else*/
        if(isroll)
        {
            isroll = false;
            rb.velocity = new Vector2(0, 0);
        }
            

        trampbefore = true;
        istramped = false;
    }

    public override void TurtleShell()
    {
        if (!isroll)
            FlipDie();
        else
        {
            currentrollspeed = -currentrollspeed;
            trutleshellscore = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isroll)
        {
            if (collision.gameObject.tag.Contains("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                manager.TurtleshellKillEnemy(enemy);
            }
            else if(collision.gameObject.tag.Contains("Player"))
            {
                manager.MarioGetSmaller();
            }
            else
                currentrollspeed = -currentrollspeed;
        }
        else
        {
            if(collision.gameObject.tag.Contains("Player"))
            {
                Vector2 normal = collision.contacts[0].normal;
                Debug.Log(normal.ToString());
                Vector2 left = new Vector2(1f, 0f);
                Debug.Log(left.ToString());
                Vector2 right = new Vector2(-1f, 0f);
                Debug.Log(right.ToString());
                if (Mathf.Abs(normal.x - left.x) < 0.1 && Mathf.Abs(normal.y - left.y) < 0.1)
                {
                    Debug.Log("乌龟壳左边碰撞");
                    if (mario.transform.localScale.x == 1)
                        currentrollspeed = rollspeed;
                    else if (mario.transform.position.x == -1)
                        currentrollspeed = -rollspeed;
                    rb.velocity = new Vector2(currentrollspeed, rb.velocity.y);
                    transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
                    // manager.MarioGetBig();
                    Debug.Log("开始滚动");
                    isroll = true;

                }
                else if (Mathf.Abs(normal.x - right.x) < 0.1 && Mathf.Abs(normal.y - right.y) < 0.1)
                {
                    Debug.Log("乌龟壳右边碰撞");
                    if (mario.transform.localScale.x == 1)
                        currentrollspeed = rollspeed;
                    else if (mario.transform.position.x == -1)
                        currentrollspeed = -rollspeed;
                    rb.velocity = new Vector2(currentrollspeed, rb.velocity.y);
                    transform.position = new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z);
                    isroll = true;
                }
                
            }
        }
    }
}
