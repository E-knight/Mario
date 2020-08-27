using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    GameStateManager manager;
    public Text topscoretext;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameStateManager>();
        manager.StartNewGame();
        int currenthighscore = PlayerPrefs.GetInt("highScore", 0);
        topscoretext.text = "Top-" + currenthighscore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseHover(Button btn)
    {
        GameObject cursor = btn.transform.Find("cursor").gameObject;
        cursor.SetActive(true);
    }
    public void OnMouseHoverExit(Button btn)
    {
        GameObject cursor = btn.transform.Find("cursor").gameObject;
        cursor.SetActive(false);
    }
    public void StartNewGame()
    {
        manager.scenename = "World 1-1";
        Debug.Log(Time.time.ToString());
        SceneManager.LoadScene("Start Level");
    }
    public void StartWorld2()
    {
        manager.scenename = "World 1-2";
        SceneManager.LoadScene("Start Level");
    }
    public void StartWorld3()
    {
        manager.scenename = "World 1-3";
        SceneManager.LoadScene("Start Level");
    }
}
