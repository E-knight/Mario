using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MonoBehaviour
{
    LevelManager manager;
    public LayerMask layerMask;

    Rigidbody2D rb;
    CircleCollider2D circleCollider;
    Animator anim;
    GameObject footbox;
    Transform groundcheck1, groundcheck2;
    
    float facedirection, movedirection;
    float mariospeed, mariospeedbeforejump;
    float autowalkspeed,autogravity;
    float deadduration = 0.3f;
   Vector2 climbspeed = new Vector2(0, -5);
    public float castlewalkspeed,levelfinishspeed;
    public bool isonground, isrun, isturn, isjump, isfall, isrunbeforejump, iscrouch, isshot,isdie,isclimbflag;
    bool holdjump, relesejump;
    bool stopinput;
    public float walkminspeed, walkmaxspeed,walkacceleration;
    public float runminspeed, runmaxspeed, runacceleration;
    public float jumpspeed, jumpupgravity, jumpdowngravity;
    public float slipspeed, slipdeceleration;
    public float midairacceleration, midairdeceleration;
    public float normalgravity, releasedeceleration;

    public Transform firepos;
    public GameObject Fireball;
    public float shotduration,firetime1,firetime2;

    
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<LevelManager>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        footbox = transform.Find("FootBox").gameObject;
        groundcheck1 = transform.Find("groundcheck1");
        groundcheck2 = transform.Find("groundcheck2");
        normalgravity = rb.gravityScale;
        transform.position = FindObjectOfType<LevelManager>().FindReveviePoint();
        UpdateSize();
        relesejump = true;
        firetime1 = firetime2 = 0;
    }
    private void FixedUpdate()
    {
        if(isonground)
        {
            if (facedirection != 0)
            {
                if (mariospeed == 0)
                    mariospeed = walkminspeed;
                else if (mariospeed < walkmaxspeed)
                    mariospeed = IncreaseSpeed(mariospeed, walkacceleration, walkmaxspeed);
                else if (isrun && mariospeed < runmaxspeed)
                    mariospeed = IncreaseSpeed(mariospeed, runacceleration, runmaxspeed);
               // Debug.Log("按键" + Time.time.ToString());
            }
            else if (mariospeed > 0)
                mariospeed = DecreaseSpeed(mariospeed, releasedeceleration, 0);
            if (isturn)
            {
                if (mariospeed > slipspeed)
                {
                    movedirection -= facedirection;
                    anim.SetBool("isstop", true);
                    mariospeed = DecreaseSpeed(mariospeed, slipdeceleration, 0);
                }
                else
                {
                    movedirection = facedirection;
                    anim.SetBool("isstop", false);
                }
            }
            else
                anim.SetBool("isstop", false);
            if (iscrouch)
                mariospeed = 0;
        }
        else
        {
            SetMidairParams();
            if (facedirection != 0)
            {
                if (mariospeed == 0)
                    mariospeed = walkminspeed;
                else if (mariospeed < walkmaxspeed)
                    mariospeed = IncreaseSpeed(mariospeed, midairacceleration, walkmaxspeed);
                else if (isrun && mariospeed < runmaxspeed)
                    mariospeed = IncreaseSpeed(mariospeed, midairacceleration, runmaxspeed);
            }
            else if (mariospeed > 0)
                mariospeed = DecreaseSpeed(mariospeed, releasedeceleration, 0);
            if(isturn)
            {
                facedirection = movedirection;
                mariospeed = DecreaseSpeed(mariospeed, midairdeceleration, 0);
            }
        }
        if(isonground)
        {
            isjump = false;
            rb.gravityScale = normalgravity;
        }
        if(!isjump)
        {
            if(isonground&&holdjump&&relesejump)
            {
                SetJumpParams();
                isjump = true;
                relesejump = false;
                rb.velocity = new Vector2(rb.velocity.x, jumpspeed);
                mariospeedbeforejump = mariospeed;
                isrunbeforejump = isrun;
                if (manager.Mariosize > 1)
                    manager.soundsource.PlayOneShot(manager.Bigjumpsound);
                else
                    manager.soundsource.PlayOneShot(manager.Smalljumpsound);
            }
        }
        else
        {
            if (rb.velocity.y > 0 && holdjump)
                rb.gravityScale = normalgravity * jumpupgravity;
            else
                rb.gravityScale = normalgravity * jumpdowngravity;
        }

        if(!isfall)
        {
            footbox.SetActive(false);
            circleCollider.enabled = true;
        }
        else
        {
            footbox.SetActive(true);
            circleCollider.enabled = false;
        }
        if (facedirection > 0)
            transform.localScale = new Vector2(1, 1);
        else if(facedirection<0)
            transform.localScale = new Vector2(-1, 1);
        if(stopinput)
        {
            mariospeed = autowalkspeed;
            rb.gravityScale = autogravity;
        }
        if(isshot&&manager.Mariosize>2)
        {
            firetime2 = Time.time;
            if(firetime2-firetime1>=shotduration)
            {
                anim.SetTrigger("isshot");
                GameObject fireball = Instantiate(Fireball, firepos.position, Quaternion.identity);
                fireball.GetComponent<Fireball>().direction = transform.localScale.x;
                firetime1 = Time.time;
                manager.soundsource.PlayOneShot(manager.Attacksound);

            }
        }

        rb.velocity = new Vector2(movedirection * mariospeed, rb.velocity.y);
        anim.SetBool("isjump", isjump);
        anim.SetBool("isfall_withoutjump", isfall && !isjump);
        anim.SetFloat("speed", Mathf.Abs(mariospeed));
        anim.SetBool("iscrouch", iscrouch);
        if (facedirection != 0 && !isturn)
            movedirection = facedirection;
    }
    // Update is called once per frame
    void Update()
    {
        if(!stopinput)
        {
            facedirection = Input.GetAxisRaw("Horizontal");
           // Debug.Log("按键" + Time.time.ToString());
            isrun = Input.GetButton("Dash");
            iscrouch = Input.GetButton("Crouch");
            holdjump = Input.GetButton("Jump");
            isshot = Input.GetButtonDown("Dash");
            if (Input.GetButtonUp("Jump"))
                relesejump = true;
        }
        if (rb.velocity.y <= 0 && !isonground)
        {
            isfall = true;
           // isjump = false ;
        }
           
        else
            isfall = false;
        if (Physics2D.OverlapPoint(groundcheck1.position, layerMask) || Physics2D.OverlapPoint(groundcheck2.position, layerMask))
            isonground = true;
        else
            isonground = false;
        if (mariospeed > 0 && facedirection * movedirection < 0)
            isturn = true;
        else
            isturn = false;
        if(stopinput&&!manager.Gamepaused)
        {
            if (isdie)
            {
                deadduration -= Time.unscaledDeltaTime;
                if (deadduration <= 0)
                    gameObject.transform.position += Vector3.down * 0.2f;
                else
                    gameObject.transform.position += Vector3.down * 0.25f;
            }
            else if (isclimbflag)
                rb.MovePosition(rb.position + climbspeed * Time.deltaTime);

        }

    }
    //更新马里奥尺寸
    public void UpdateSize()
    {
        GetComponent<Animator>().SetInteger("mario_size", FindObjectOfType<LevelManager>().Mariosize);
        //isonground = true;
    }
    //设置跳跃参数
    public void SetJumpParams()
    {
        if(mariospeed<4)
        {
            jumpupgravity = 1.1f;
            jumpdowngravity = 1.5f;
            jumpspeed = 10;
        }
        else if(mariospeed<8)
        {
            jumpupgravity = 1.253f;
            jumpdowngravity = 1.8f;
            jumpspeed = 11;
        }
    }
    //设置半空中参数
    public void SetMidairParams()
    {
        if(mariospeed<6)
        {
            midairacceleration = 0.2f;
            if (mariospeedbeforejump < 7)
                midairdeceleration = 0.2f;
            else
                midairdeceleration = 0.25f;
        }
        else
        {
            midairdeceleration = 0.23f;
            midairacceleration = 0.23f;
        }
    }

    //增加速度
    private float IncreaseSpeed(float speed,float acceleration,float maxspeed=Mathf.Infinity)
    {
        speed += acceleration;
        if (speed >= maxspeed)
            speed = maxspeed;
        return speed;
    }
    //降低速度
    private float DecreaseSpeed(float speed,float deceration,float minspeed=0)
    {
        speed -= deceration;
        if (speed <= minspeed)
            speed = minspeed;
        return speed;
    }
    //停止用户输入
    public void StopUserInput()
    {
        stopinput = true;
        relesejump = true;
        holdjump = false;
        isrun = false;
        isrunbeforejump = false;
        iscrouch = false;
        isturn = false;
        isshot = false;

        movedirection = 0;
        facedirection = 0;
        mariospeed = 0;
        mariospeedbeforejump = 0;
        autogravity = normalgravity;
        autowalkspeed = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }
    //停止用户输入并死亡
    public void StopDie()
    {
        StopUserInput();
        isdie = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        anim.SetTrigger("isdie");
       // gameObject.layer = LayerMask.NameToLayer("Fall Kill Plane");
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground Effect";

    }
    //自动行走
    public void AutoWalk(float walkspeed)
    {
        StopUserInput();
        if (walkspeed != 0)
            facedirection = walkspeed / Mathf.Abs(walkspeed);
        autowalkspeed = Mathf.Abs(walkspeed);
    }
    //自动下蹲
    public void AutoCrouch()
    {
        StopUserInput();
        iscrouch = true;
    }
    //爬旗杆
    public void ClimbFlag()
    {
        StopUserInput();
        isclimbflag = true;
        Debug.Log("正在爬旗杆");

        anim.SetBool("isclimb", true);
        //rb.bodyType = RigidbodyType2D.Kinematic;
    }
    //离开旗杆
    public void LeaveFlag()
    {
        Debug.Log("离开旗杆");
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y);
        anim.SetBool("isclimb", false);
        
        AutoWalk(castlewalkspeed);
rb.bodyType = RigidbodyType2D.Dynamic;
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("马里奥开始碰撞");
        Vector2 normal = collision.contacts[0].normal;
        Vector2 bottomside = new Vector2(0f, 1f);
        Debug.Log("法线：" + normal.ToString());
        Vector2 v = normal - bottomside;
        bool bottomhit = false;
        if (Mathf.Abs(v.x) < 0.1 && Mathf.Abs(v.y) < 0.1)
            bottomhit = true;
        //bool bottomhit=normal==bottomside;
       /* if (normal == bottomside)
            bottomhit = true;
        else
            bottomhit = false;*/
        //Debug.Log("isclimb:" + isclimbflag.ToString());
        Debug.Log("bottomhit:" + bottomhit.ToString());
        if (collision.gameObject.tag.Contains("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Debug.Log(enemy.name);
            /*normal = collision.contacts[0].normal;
            bottomside = new Vector2(0f, 1f);
            Debug.Log("法线：" + normal.ToString());
            v = normal - bottomside;
            bottomhit = false;
            if (Mathf.Abs(v.x) < 0.1 && Mathf.Abs(v.y) < 0.1)
                bottomhit = true;
            Debug.Log("bottomhit:" + bottomhit.ToString());*/
            if (!manager.isinvincible())
            {

                //Debug.Log(collision.gameObject.GetComponent<KoopaShell>());
                if (collision.gameObject.GetComponent<TrutleShell>() && !collision.gameObject.GetComponent<TrutleShell>().isroll)
                {
                    Debug.Log("碰撞静止龟壳");
                    //manager.MarioGetSmaller();
                }
                else if (!collision.gameObject.GetComponent<TrutleShell>())
                {
                    //Debug.Log(bottomhit.ToString());
                    Debug.Log("碰撞非乌龟敌人");
                    manager.MarioGetSmaller();
                }
                /*else if(collision.gameObject.GetComponent<TrutleShell>().isroll)
                {
                    //if(Time.time- collision.gameObject.GetComponent<TrutleShell>().shelltime>3f)
                   // {
                        Debug.Log("碰撞移动龟壳");
                    manager.MarioGetSmaller();

                 //   }
                   
                }*/
                else if(!collision.gameObject.GetComponent<TrutleShell>().isroll&&!bottomhit)
                {
                    Debug.Log("非底部碰撞");
                    manager.MarioGetSmaller();
                }
                else if(!enemy.istramped&&bottomhit)
                {
                    Debug.Log("敌人不被踩");
                    manager.MarioGetSmaller();
                }
            }
            else if (manager.isinvinciblecolor)
            {
                Debug.Log("无敌马里奥碰撞");
                    manager.StarmanKillEnemy(enemy);
            }
               
        }
        else if(collision.gameObject.tag=="Goal"&&isclimbflag&&bottomhit)
        {
            Debug.Log("跳下旗杆");
            isclimbflag = false;
            LeaveFlag();
        }
    }
}
