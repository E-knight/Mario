using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
public class GameOver : MonoBehaviour
{
    GameStateManager manager;
    public Text scoretext;
    public Text cointext;
    public Text worlstext;
    public Text message;
    public AudioSource musicsource;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameStateManager>();
        string worldname = manager.scenename;
        scoretext.text = manager.score.ToString("D6");
        cointext.text = "x" + manager.coins.ToString("D2");
        worlstext.text = Regex.Split(worldname, "World ")[1];
        Time.timeScale = 1;
        bool timeover = manager.timeover;
        if (!timeover)
            message.text = "GAME OVER";
        else
            StartCoroutine(ChangeTextCoroutine());
        musicsource.volume = 1;
        musicsource.Play();
        LoadMainMenu(musicsource.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Pause"))
            LoadMainMenu();
    }
    IEnumerator LoadSceneCoroutine(string scenename, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(scenename);
    }
    IEnumerator ChangeTextCoroutine()
    {
        message.text = "TIME UP";
        yield return new WaitForSecondsRealtime(1);
        message.text = "GAME OVER";
    }
    void LoadMainMenu(float delay=0)
    {
        StartCoroutine(LoadSceneCoroutine("Main Menu", delay));
    }
}
