using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Start is called before the first frame up14date
    public int lives;
    public int coins;
    public int score;
    public float timeremain;
    public int mariosize;
    public bool ishurry;
    public bool revivefrompoint;
    public bool timeover;
    public int revivepointx;
    public int revivepipex;
    public string scenename;

    //设置复活点
    public void ResetRevivePosition()
    {
        revivefrompoint = true;
        revivepointx = 0;
        revivepipex = 0;
    }
    //开始新游戏
    public void StartNewGame()
    {
        lives = 3;
        mariosize = 1;
        score = 0;
        coins = 0;
        timeremain = 999;
        scenename = "";
        timeover = false;
        ResetRevivePosition();
        ishurry = false;

    }
    //开始新关卡
    public void StartNewLevel()
    {
        timeremain = 999;
        ishurry = false;
        ResetRevivePosition();
    }
    //重玩关卡
    public void StartCurrentLevel()
    {
        timeremain = 999;
        ishurry = false;
    }
    public void SetRevivePipe(int x)
    {
        revivefrompoint = false;
        revivepipex = x;
    }
    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length == 1)
        {
            DontDestroyOnLoad(gameObject);//使对象不随场景切换而被摧毁
            StartNewGame();
        }
        else
            Destroy(gameObject);
    }
    public void SaveData()
    {
        LevelManager manager = FindObjectOfType<LevelManager>();
        lives = manager.lives;
        score = manager.scores;
        coins = manager.coins;
        timeremain = manager.Time_remain;
        ishurry = manager.ishurry;
        mariosize = manager.Mariosize;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
