using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVerticalBirth : MonoBehaviour
{
    public GameObject movingplatform;
    public bool ismove;
    public float direction = -1;
    public Transform up,down,birthpalce;
    float waittime = 1.5f, movedistance = 40;
    GameObject mario;
    MoveVertical move;
    float duration;
    // Start is called before the first frame update
    void Start()
    {
        mario = FindObjectOfType<Mario>().gameObject;
        duration = waittime / 2;
        ismove = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(mario.transform.position.x - transform.position.x) <= movedistance)
            ismove = true;
        else
            ismove = false;
        if (ismove)
            duration -= Time.deltaTime;
        if(duration<=0)
        {
            GameObject clone = Instantiate(movingplatform, birthpalce.position, Quaternion.identity);
            move = clone.GetComponent<MoveVertical>();
            move.up = up;
            move.down = down;
            move.direction = direction;
            move.canmove = true;
            duration = waittime;
        }
    }
}
