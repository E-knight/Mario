using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
public class TimeOver : MonoBehaviour
{
    GameStateManager manager;
    public Text scoretext;
    public Text cointext;
    public Text worlstext;
    float delay = 2;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameStateManager>();
        string worldname = manager.scenename;
        scoretext.text = manager.score.ToString("D6");
        cointext.text = "x"+manager.coins.ToString("D2");
        worlstext.text = Regex.Split(worldname, "World ")[1];
        Time.timeScale = 1;
        StartCoroutine(LoadSceneCoroutine("Start Level", delay));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadSceneCoroutine(string scenename, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(scenename);
    }
}
