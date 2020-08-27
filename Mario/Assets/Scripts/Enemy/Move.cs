using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private bool canmove = false;
    [SerializeField] private bool automove = true;
    public float startmove_dis = 15;
    public float direction = -1;
    public Vector2 v = new Vector2(4, 0);
    public bool Hitside;
    private GameObject mario;
    private Rigidbody2D rb;
    float time1;
    // Start is called before the first frame update
    void Start()
    {
        mario = FindObjectOfType<Mario>().gameObject;
        rb = GetComponent<Rigidbody2D>();
        HorizotalFlip();
        time1 = Time.time;
    }

    void FixedUpdate()
    {
        if(canmove)
        {
            Vector2 speed = new Vector2(v.x * direction, rb.velocity.y);
            rb.velocity = speed;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!canmove&& automove == true && startmove_dis >= Mathf.Abs(mario.transform.position.x - transform.position.x))
            canmove = true;
       // if(Time.time-time1>=13f)
       // {
       //     direction = -direction;
           // time1 = Time.time;
       // }
    }

    void HorizotalFlip()
    {
        if(direction>0)
        {
            Vector3 scale = new Vector3(1, 1, 1);
            transform.localScale = scale;
        }
        else if(direction<0)
        {
            Vector3 scale = new Vector3(-1, 1, 1);
            transform.localScale = scale;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Debug.Log(normal.ToString());
        Vector2 left = new Vector2(-1f, 0f);
        Vector2 right = new Vector2(1f, 0f);
        Vector2 bottom = new Vector2(0f, 1f);
        Debug.Log(normal.x.ToString() +":"+ normal.y.ToString());
        Vector2 v1 = normal - left;
        if (Mathf.Abs(v1.x) < 0.1 && Mathf.Abs(v1.y) < 0.1)
            Hitside = true;
        Vector2 v2 = normal - right;
        if (Mathf.Abs(v2.x) < 0.1 && Mathf.Abs(v2.y) < 0.1)
            Hitside = true;
       // Hitside = (normal == left) || normal == right;
        //Debug.Log();
        bool Hitbottom = false;
        if (normal == bottom)
            Hitbottom = true;
        Debug.Log(collision.gameObject.tag+":"+Hitside.ToString());
        if(collision.gameObject.tag!="Player"&&Hitside)
        {
            direction = -direction;
            Debug.Log("翻转");
            HorizotalFlip();
            //v.x = -v.x;
        }
        else if(collision.gameObject.tag.Contains("Platform")&&Hitbottom&&canmove)
        {
            rb.velocity = new Vector2(rb.velocity.x, v.y);
        }
    }
}
