using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : MonoBehaviour
{
    LevelManager manager;
    Animator anim;
    public GameObject item;
    public GameObject mushroom;
    public GameObject flower;
    public bool ispowerblock;
    public int duration;
    public Vector3 offset;
   float bouncetime;
    bool isactive;
    float time1,time2;
    public List<GameObject> enemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        bouncetime = 0.15f;
        isactive = true;
        anim = GetComponent<Animator>();
        manager = FindObjectOfType<LevelManager>();
        time1 = Time.time;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        time2 = Time.time;
        if(collision.tag=="Player"&&time2-time1>=bouncetime)
        {
            manager.soundsource.PlayOneShot(manager.Bumpsound);
            if(isactive)
            {
                anim.SetTrigger("bounce");
                foreach (GameObject enemy in enemies)
                    manager.EnemyDrop(enemy.GetComponent<Enemy>());
                if(duration>0)
                {
                    if(ispowerblock)
                    {
                        if (manager.Mariosize == 1)
                            item = mushroom;
                        else
                            item = flower;
                    }
                    Instantiate(item, transform.position + offset, Quaternion.identity);
                    duration--;
                    if(duration==0)
                    {
                        anim.SetTrigger("deactive");
                        isactive = false;
                    }
                }
            }
            time1 = Time.time;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 top = new Vector2(0, -1);
        Vector2 v = normal - top;
        bool tophit;
        if (Mathf.Abs(v.x)<0.05&&Mathf.Abs(v.y)<0.05)
            tophit = true;
        else
            tophit = false;
        if (collision.gameObject.tag.Contains("Enemy") && tophit && !enemies.Contains(collision.gameObject))
            enemies.Add(collision.gameObject);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Enemy"))
            enemies.Remove(collision.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
