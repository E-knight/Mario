using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int Coinscore = 200;
    [SerializeField] private int Powerupscore=1000;
    [SerializeField] private int Starmanscore = 1000;
    [SerializeField] private int Oneupscore = 100;
    public int Blockbreakscore=50;

    public int Mariosize;//马里奥尺寸
    public int scores;//分数
    public int coins;//金币数
    public float Time_remain;//剩余时间
    public int Time_remain2;//剩余时间整数
    public int lives;//生命数

    public bool ishurry;//是否正在加速
    private bool issmaller;//是否变小
    private bool isdie;//复活
    public  bool isinvinciblecolor;//无敌状态变色
    private bool isinvinciblepower;//无敌状态
    public bool Gamepaused;
    private bool Timepaused;
    private bool Musicpaused;

    private const float loadscenetime = 1.0f;//加载场景时间
    private float powerdowntime = 2.0f;//变小时间
    private float starmantime = 12.0f;//无敌时间
    private float transformtime = 1.0f;//变化时间

    public Text scoretext;//分数显示
    public Text cointext;//金币数显示
    public Text timetext;//时间显示

    private GameStateManager gamemanager;
    private Mario mario;

    private Animator anim;
    private Rigidbody2D rb;
    private Vector2 Footnouncevelocity = new Vector2(0, 1);
    public GameObject Floateffect;
    private float floaty = 1.0f;

    public AudioSource musicsource;
    public AudioSource soundsource;
    public AudioSource pausesource;

    public AudioClip Mainmusic;
    public AudioClip Mainmusic_hurry;
    public AudioClip Starmanmusic;
    public AudioClip Starmanmusic_hurry;
    public AudioClip Levelcompletemusic;
    public AudioClip Castlecompletemusic;
    public AudioClip Browserfallsound;
    public AudioClip Browserfiresound;
    public AudioClip Blockbreaksound;
    public AudioClip Oneupsound;
    public AudioClip Smalljumpsound;
    public AudioClip Bigjumpsound;
    public AudioClip Collectcoinsound;
    public AudioClip Bumpsound;
    public AudioClip Deadsound;
    public AudioClip Attacksound;
    public AudioClip powerdownsound;
    public AudioClip Powerupsound;
    public AudioClip Powerappearsound;
    public AudioClip Trampsound;
    public AudioClip Warnsound;
    public AudioClip Kicksound;
    public AudioClip Flagsound;
    List<Animator> li;
    float timescalebeforepause;
    bool musicpausebeforepause;
    void Awake()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
        Debug.Log("进入场景"+Time.time.ToString());
        gamemanager = FindObjectOfType<GameStateManager>();
        Restoregamedata();
        mario = FindObjectOfType<Mario>();
        anim = mario.gameObject.GetComponent<Animator>();
        rb = mario.gameObject.GetComponent<Rigidbody2D>();
        mario.UpdateSize();

        musicsource.volume = 1;
        soundsource.volume = 1;
        pausesource.volume = 1;
      
        Score_display();
        Coin_display();
        Time_display();

        if (ishurry)
            ChangeMusic(Mainmusic_hurry);
        else
        {
            //Debug.Log("播放音乐");
              ChangeMusic(Mainmusic);
        }
           

    }

    // Update is called once per frame
    void Update()
    {
        //时间不够了
        if(Time_remain<100&&ishurry==false)
        {
            ishurry = true;
            StopMusicPlaySound(Warnsound, true);
            if (!isinvinciblecolor)
                ChangeMusic(Mainmusic_hurry, Warnsound.length);
            else
                ChangeMusic(Starmanmusic_hurry, Warnsound.length);
        }

        //时间用完
        if (Time_remain<=0)
        {
            MarioDie(true);
        }
        //按下暂停键
        if(Input.GetButtonDown("Pause"))
        {
            Debug.Log("按下暂停键");
            if (!Gamepaused)
                StartCoroutine(PauseGameCoroutine());
            else
                StartCoroutine(UnpauseCoroutine());
        }
        //刷新时间
        if(!Timepaused)
        {
            Time_remain -= Time.deltaTime / 0.2f;
            Time_display();
        }
    }

    //恢复游戏数据
    void Restoregamedata()
    {
        Mariosize = gamemanager.mariosize;
        scores = gamemanager.score;
        coins = gamemanager.coins;
        lives = gamemanager.lives;
        Time_remain = gamemanager.timeremain;
        ishurry = gamemanager.ishurry;
    }

    //游戏分数显示
    void Score_display()
    {
        scoretext.text = scores.ToString("D6");
    }

    //游戏金币显示
    void Coin_display()
    {
        cointext.text = "x" + coins.ToString("D2");
    }

    //游戏时间显示
    void Time_display()
    {
        Time_remain2 = Mathf.RoundToInt(Time_remain);
        timetext.text = Time_remain2.ToString("D3");
    }
    //玩家死亡
    public void MarioDie(bool notime=false)
    {
        if(!isdie)
        {
           
            lives = lives - 1;
            Mariosize = 1;
            isdie = true;
            soundsource.Stop();
            musicsource.Stop();
            Musicpaused = true;
            soundsource.PlayOneShot(Deadsound);
            Time.timeScale = 0.0f;
            mario.StopDie();
            if(lives>0)
            {
                ReloadCurrentScene(Deadsound.length, notime);
            }
            else
            {
                LoadGameOverScene(Deadsound.length, notime);
                //跳转到游戏结束场景
            }
        }
    }
    //切换音乐协程
    IEnumerator ChangeMusicCoroutine(AudioClip music,float duration)
    {
        musicsource.clip = music;
        yield return new WaitWhile(() => Gamepaused);
        yield return new WaitForSecondsRealtime(duration);
        yield return new WaitWhile(() => Gamepaused||Musicpaused);
        if (isdie == false)
        {
            Debug.Log("切换音乐");
            musicsource.Play();
        }
           
    }
    //切换音乐
    public void ChangeMusic(AudioClip music,float duration=0.0f)
    {
        StartCoroutine(ChangeMusicCoroutine(music, duration));
    }

    //暂停音乐播放声音协程
    IEnumerator StopMusicPlaySoundCoroutine(AudioClip music,bool restartmusic)
    {
        string musicname;
        if (musicsource.clip)
            musicname = musicsource.clip.name;
        musicsource.Pause();
        Musicpaused = true;
        soundsource.PlayOneShot(music);
        yield return new WaitForSeconds(music.length);
        if (restartmusic)//   musicname = musicsource.clip.name;
        {
            musicsource.UnPause();
            musicname = "";
            if (musicsource.clip)
                musicname = musicsource.clip.name;
            //Musicpaused = false;
        }
        Musicpaused = false;
    }

    //暂停音乐播放声音
    public void StopMusicPlaySound(AudioClip music,bool restartmusic)
    {
        StartCoroutine(StopMusicPlaySoundCoroutine(music, restartmusic));
    }
    
    //暂停游戏协程
    IEnumerator PauseGameCoroutine()
    {
      
        timescalebeforepause = Time.timeScale;
        musicpausebeforepause = Musicpaused;
        Gamepaused = true;
        Time.timeScale = 0;
        Musicpaused = true;
        musicsource.Pause();
        soundsource.Pause();
        li.Clear();
        foreach(Animator anim in FindObjectsOfType<Animator>())
        {
            if(anim.updateMode==AnimatorUpdateMode.UnscaledTime)
            {
                li.Add(anim);
                anim.updateMode = AnimatorUpdateMode.Normal;
            }
        }
        pausesource.Play();
        yield return new WaitForSecondsRealtime(pausesource.clip.length);
    }

    //继续游戏协程
    IEnumerator UnpauseCoroutine()
    {
        pausesource.Play();
        yield return new WaitForSecondsRealtime(pausesource.clip.length);
        Musicpaused = musicpausebeforepause;
        if (!Musicpaused)
            musicsource.UnPause();
        soundsource.UnPause();
        foreach (Animator anim in li)
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        li.Clear();
        Gamepaused = false;
        Time.timeScale = timescalebeforepause;
    }
    //马里奥升级
    public void MarioGetBig()
    {
        soundsource.PlayOneShot(Powerupsound);
       
        if (Mariosize <= 2)
            StartCoroutine(MarioGetBigCoroutine());
       // AddScore()
        AddScore(Powerupscore, mario.transform.position);
    }
    //马里奥升级协程
    IEnumerator MarioGetBigCoroutine()
    {
        
        anim.SetBool("isbigger", true);
        Time.timeScale = 0;
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSecondsRealtime(transformtime);
        yield return new WaitWhile(() => Gamepaused);
        Time.timeScale = 1;
        anim.updateMode = AnimatorUpdateMode.Normal;
        Mariosize ++;

        mario.UpdateSize();
        anim.SetBool("isbigger", false);

    }
    //马里奥降级协程
    IEnumerator MarioGetSmallerCoroutine()
    {
        Time.timeScale = 0;
        anim.SetBool("issmaller", true);
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSecondsRealtime(transformtime);
        yield return new WaitWhile(() => Gamepaused);
        Time.timeScale = 1;
        anim.updateMode = AnimatorUpdateMode.Normal;
        MarioStarmandown();
        Mariosize = 1;
        mario.UpdateSize();
        issmaller = false;
        anim.SetBool("issmaller", false);
  
    }
    //马里奥降级
    public void MarioGetSmaller()
    {
        if(!issmaller)
        {
            issmaller = true;
            if (Mariosize >= 2)
            {
                StartCoroutine(MarioGetSmallerCoroutine());
                soundsource.PlayOneShot(powerdownsound);
            }
            else
            {
                Debug.Log("死亡");
                MarioDie();
            }
                
        }
    }
    //马里奥踩敌人
    public void MarioTrampEnemy(Enemy enemy)
    {
        rb.velocity = new Vector2(rb.velocity.x + Footnouncevelocity.x, Footnouncevelocity.y);
        enemy.Tramp();
        soundsource.PlayOneShot(Trampsound);
        AddScore(enemy.trampscore, enemy.gameObject.transform.position);

    }

    //敌人接触无敌马里奥
    public void StarmanKillEnemy(Enemy enemy)
    {
        enemy.Starman();
        soundsource.PlayOneShot(Kicksound);
        AddScore(enemy.trampscore, enemy.gameObject.transform.position);
    }
    //敌人接触龟壳
    public void TurtleshellKillEnemy(Enemy enemy)
    {
        enemy.TurtleShell();
        soundsource.PlayOneShot(Kicksound);
        AddScore(enemy.trutleshellscore, enemy.gameObject.transform.position);
    }
    //敌人被火球砸中
    public void FireballEnemy(Enemy enemy)
    {
        enemy.Fireball();
        soundsource.PlayOneShot(Kicksound);
        AddScore(enemy.firehitscore, enemy.gameObject.transform.position);
    }
    //敌人掉下去
    public void EnemyDrop(Enemy enemy)
    {
        enemy.Drop();
        AddScore(enemy.dropscore, enemy.gameObject.transform.position);
    }
   
    //是否无敌
    public bool isinvincible()
    {
        if (isinvinciblecolor || isinvinciblepower)
            return true;
        return false;
    }

    //进入无敌状态协程
    IEnumerator MariostarmanCoroutine()
    {
        anim.SetBool("isinvinciblecolor", true);
        isinvinciblecolor = true;
        mario.gameObject.layer = LayerMask.NameToLayer("Starman");
        if (ishurry)
            ChangeMusic(Mainmusic_hurry);
        else
            ChangeMusic(Mainmusic);
        yield return new WaitForSeconds(starmantime);
        anim.SetBool("isinvinciblecolor", false);
        isinvinciblecolor = false;
        mario.gameObject.layer = LayerMask.NameToLayer("Mario");
        if (ishurry)
            ChangeMusic(Mainmusic_hurry);
        else
            ChangeMusic(Mainmusic);
    }
    //进入无敌状态
    public void Mariostarman()
    {
        StartCoroutine(MariostarmanCoroutine());
        AddScore(Starmanscore, mario.transform.position);
    }
    //创建浮动文字
    public void CreateFloatText(string text,Vector3 pos)
    {
        GameObject texteffect = Instantiate(Floateffect, pos, Quaternion.identity);
        texteffect.GetComponentInChildren<TextMesh>().text = text.ToUpper();
    }
    //增加生命
    public void AddLife()
    {
        lives++;
        soundsource.PlayOneShot(Oneupsound);

    }
    public void AddLife(Vector3 pos)
    {
        lives++;
        soundsource.PlayOneShot(Oneupsound);
        CreateFloatText("1UP", pos);
    }
    //增加金币
    public void AddCoin()
    {
        coins++;
        soundsource.PlayOneShot(Collectcoinsound);
        if(coins>=100)
        {
            coins -= 100;
            AddLife();
        }
        Coin_display();
        AddScore(Coinscore);
    }
    public void AddCoin(Vector3 pos)
    {
        coins++;
        soundsource.PlayOneShot(Collectcoinsound);
        if (coins >= 100)
        {
            coins -= 100;
            AddLife();
        }
        Coin_display();
        AddScore(Coinscore,pos);
    }
    //增加分数
    public void AddScore(int addscore)
    {
        scores += addscore;
        Score_display();
    }
    public void AddScore(int addscore,Vector3 pos)
    {
        scores += addscore;
        Score_display();
        if (addscore > 0)
            CreateFloatText(addscore.ToString(), pos);
    }
    
    //无敌状态变弱
    public void MarioStarmandown()
    {
        StartCoroutine(MarioStarmandownCoroutine());
    }
    //无敌状态变弱协程
    IEnumerator MarioStarmandownCoroutine()
    {
        anim.SetBool("isinvinciblepower", true);
        isinvinciblepower = true;
        mario.gameObject.layer = LayerMask.NameToLayer("Powerdown Mario");
        yield return new WaitForSeconds(powerdowntime);
        anim.SetBool("isinvinciblepower", false);
        isinvinciblepower = false;
        mario.gameObject.layer = LayerMask.NameToLayer("Mario");
    }
    //加载场景协程
    IEnumerator LoadSceneDelayCoroutine(string scenename,float duration)
    {
        float waittime = 0;
        while(waittime<duration)
        {
            if (Gamepaused == false)
                waittime += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return new WaitWhile(() => Gamepaused);
        isdie = false;
        issmaller = false;
        SceneManager.LoadScene(scenename);
    }
    //加载场景
    public void LoadSceneDelay(string scenename,float duration=loadscenetime)
    {
        Timepaused = true;
        StartCoroutine(LoadSceneDelayCoroutine(scenename, duration));
    }
    //加载新关卡
    public void LoadNewLevel(string scenename, float duration = loadscenetime)
    {
        gamemanager.SaveData();
        gamemanager.StartNewLevel();
        gamemanager.scenename = scenename;
        if (scenename != "Main Menu")
            LoadSceneDelay("Start Level", duration);
        else
            LoadSceneDelay("Main Menu", duration);

    }
    //加载本关卡
    public void LoadCurrentScene(string scenename, float duration = loadscenetime)
    {
        gamemanager.SaveData();
        gamemanager.ResetRevivePosition();
        LoadSceneDelay(scenename, duration);
    }
    //加载本关卡管道
    public void LoadCurrentScenePipe(string scenename,int x,float duration = loadscenetime)
    {
        gamemanager.SaveData();
        gamemanager.SetRevivePipe(x);
        LoadSceneDelay(scenename, duration);
    }
    //重新加载本场景
    public void ReloadCurrentScene(float duration = loadscenetime,bool timeover=false)
    {
        gamemanager.SaveData();
        gamemanager.StartCurrentLevel();
        gamemanager.scenename = SceneManager.GetActiveScene().name;
        if (timeover)
            LoadSceneDelay("Time Over", duration);
        else
            LoadSceneDelay("Start Level", duration);
    }
    //加载游戏结束场景
    public void LoadGameOverScene(float duration = loadscenetime, bool timeover = false)
    {
        int currentscore = PlayerPrefs.GetInt("highscore", 0);
        if (scores > currentscore)
            PlayerPrefs.SetInt("highscore", scores);
        gamemanager.timeover = timeover;
        LoadSceneDelay("Game Over", duration);
    }
    
    //寻找复活点
    public Vector3 FindReveviePoint()
    {
        GameStateManager manager = FindObjectOfType<GameStateManager>();
        Vector3 pos;
        if(manager.revivefrompoint)
            pos= GameObject.Find("Revive Points").transform.GetChild(manager.revivepointx).transform.position;
        else
            pos= GameObject.Find("Revive Pipes").transform.GetChild(manager.revivepipex).transform.Find("Revive Pos").transform.position;
        return pos;
    }
    //获取世界名称
    public string GetNameOfWorld(string scenename)
    {
        string[] scenenameparts = Regex.Split(scenename, " - ");
        return scenenameparts[0];
    }
    //马里奥完成关卡
    public void FinishLevel()
    {
        ChangeMusic(Levelcompletemusic);
        musicsource.loop = false;
        Timepaused = true;
    }
    //马里奥城堡自走
    public void FinishCastle()
    {

        ChangeMusic(Castlecompletemusic);
        musicsource.loop = false;
        Timepaused = true;
        mario.AutoWalk(mario.castlewalkspeed);
    }
    //马里奥接触旗杆
    public void MarioFlag()
    {
        Timepaused = true;
        StopMusicPlaySound(Flagsound, false);
        mario.ClimbFlag();
    }
}
