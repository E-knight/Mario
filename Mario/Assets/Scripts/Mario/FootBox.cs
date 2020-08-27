using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootBox : MonoBehaviour
{
    LevelManager levelmanager;
    // Start is called before the first frame update
    void Start()
    {
        levelmanager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag.Contains("Enemy")&&collision.gameObject.tag!="Enemy/Piranha"&&collision.gameObject.tag!="Enemy/Bowser")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            levelmanager.MarioTrampEnemy(enemy);
        }
    }
}
