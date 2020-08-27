using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
public class StartLevel : MonoBehaviour
{
    GameStateManager manager;
    public Text scoretext;
    public Text cointext;
    public Text worldtext;
    public Text worldtext2;
    public Text lifetext;
    float delay;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameStateManager>();
        scoretext.text = manager.score.ToString("D6");
        cointext.text = "x"+manager.coins.ToString("D2");
        lifetext.text = "x"+manager.lives.ToString();
        string worldname = manager.scenename;
        worldtext.text = Regex.Split(worldname, "World ")[1];
        worldtext2.text = Regex.Split(worldname, "World ")[1];
        Time.timeScale = 1;
        Debug.Log(Time.time.ToString());
        StartCoroutine(LoadSceneCoroutine(manager.scenename, delay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadSceneCoroutine(string scenename,float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(scenename);
    }
}
