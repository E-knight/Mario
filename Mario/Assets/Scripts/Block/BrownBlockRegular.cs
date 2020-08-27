using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownBlockRegular : MonoBehaviour
{
    public GameObject TempCollider;
    public GameObject BlockCoin;
    public GameObject BrickPiece;

    LevelManager manager;
    Animator anim;
    CoinDecetor decetor;
    float bounceduration = 0.25f;
    float time1, time2;
    List<GameObject> enemies = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        anim = GetComponent<Animator>();
        decetor = transform.parent.Find("Coin Decetor").GetComponent<CoinDecetor>();
        time1 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //砖块被撞成碎片
    void BreakBrickPiece()
    {
        GameObject brickpiece;
        brickpiece = Instantiate(BrickPiece, transform.position, Quaternion.Euler(new Vector3(45, 0, 0)));
        brickpiece.GetComponent<Rigidbody2D>().velocity = new Vector2(3, 12);
        Destroy(brickpiece.gameObject, 0.25f);
        brickpiece = Instantiate(BrickPiece, transform.position, Quaternion.Euler(new Vector3(45, 0, 0)));
        brickpiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-3, 12);
        Destroy(brickpiece.gameObject, 0.25f);
        brickpiece = Instantiate(BrickPiece, transform.position, Quaternion.Euler(new Vector3(45, 0, 0)));
        brickpiece.GetComponent<Rigidbody2D>().velocity = new Vector2(3, 8);
        Destroy(brickpiece.gameObject, 0.25f);
        brickpiece = Instantiate(BrickPiece, transform.position, Quaternion.Euler(new Vector3(45, 0, 0)));
        brickpiece.GetComponent<Rigidbody2D>().velocity = new Vector2(-3, 8);
        Destroy(brickpiece.gameObject, 0.25f);
        Instantiate(TempCollider, transform.position, Quaternion.identity);
        Destroy(transform.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        time2 = Time.time;
        if(collision.tag=="Player"&&time2-time1>=bounceduration)
        {
            foreach (GameObject enemy in enemies)
                manager.EnemyDrop(enemy.GetComponent<Enemy>());
            if(decetor.coin)
            {
                Instantiate(BlockCoin, transform.position + new Vector3(0,2,0), Quaternion.identity);
                Destroy(decetor.coin);
            }
            if(manager.Mariosize<2)
            {
                Debug.Log("撞砖");
                anim.SetTrigger("bounce");
                manager.soundsource.PlayOneShot(manager.Bumpsound);
            }
            else
            {
                BreakBrickPiece();
                manager.soundsource.PlayOneShot(manager.Blockbreaksound);
                manager.AddScore(manager.Blockbreakscore);
            }
            time1 = Time.time;
        }
      

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 topside = new Vector2(0, -1);
        bool tophit;
        if (normal == topside)
            tophit = true;
        else
            tophit = false;
        if (collision.gameObject.tag.Contains("Enemy")&&tophit&&!enemies.Contains(collision.gameObject))
            enemies.Add(collision.gameObject);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Enemy"))
            enemies.Remove(collision.gameObject);
    }
}
