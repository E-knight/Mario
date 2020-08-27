using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector2 velocityflip = new Vector2(0, 5);
    public bool istramped;
    public int trampscore;
    public int dropscore;
    public int firehitscore;
    public int trutleshellscore;
    public int starmanscore;
    //开始移动
    public void StartMove()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        foreach (Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = true;
        }
    }

    //停止移动
    public void StopMove()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        foreach(Collider2D collider in GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }
    }
    //翻转死亡
    public virtual void FlipDie()
    {
        Debug.Log("死亡");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Animator anim = GetComponent<Animator>();
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName="Enemy";
        rb.velocity = rb.velocity + velocityflip;
        anim.SetTrigger("flip");
        //gameObject.layer = LayerMask.NameToLayer("Kill Plane");
        Destroy(gameObject);
    }
   
    //被玩家踩踏
    public virtual void Tramp()
    {

    }

    //掉下去
    public virtual void Drop()
    {
        FlipDie();
    }

    //被火球击中
    public virtual void Fireball()
    {
        FlipDie();
    }
    //被龟壳砸到
    public virtual void TurtleShell()
    {
        FlipDie();
    }

    //被无敌马里奥碰到
    public virtual void Starman()
    {
        FlipDie();
    }
}
