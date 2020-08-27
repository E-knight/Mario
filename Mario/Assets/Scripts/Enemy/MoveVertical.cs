using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVertical : MonoBehaviour
{
    GameObject mario;
    public bool canmove=false,automove=true;
    public Transform up, down;
    public float speed,speedmodify=1,direction=1;
    float movedistance = 14;
    public float upwait, downwait;
    public bool atup, atdown;
    bool upstartwait, downstartwait;
    float currentspeed;

    // Start is called before the first frame update
    void Start()
    {
        mario = FindObjectOfType<Mario>().gameObject;
        if (transform.position.y >= up.position.y)
            direction = -1;
        else if (transform.position.y <= down.position.y)
            direction = 1;
        currentspeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canmove && Mathf.Abs(mario.transform.position.x - transform.position.x) <= movedistance)
            canmove = true;
        else if(canmove&&Time.timeScale!=0)
        {
            if(!atdown&&!atup)
            {
                currentspeed *= speedmodify;
                transform.position += new Vector3(0, currentspeed * direction, 0);
                if (transform.position.y >= up.position.y)
                    atup = true;
                if (transform.position.y <= down.position.y)
                    atdown = true;
            }
            else if(atup&&!upstartwait)
            {
                StartCoroutine(Waitatup());
                upstartwait = true;
            }
            else if(atdown&&!downstartwait)
            {
                StartCoroutine(Waitatdown());
                downstartwait = true;
            }
        }
    }
    IEnumerator Waitatup()
    {
        yield return new WaitForSeconds(upwait);
        currentspeed = speed;
        direction = -1;
        atup = false;
        upstartwait = false;
    }
    IEnumerator Waitatdown()
    {
        yield return new WaitForSeconds(downwait);
        currentspeed = speed;
        direction = 1;
        atdown = false;
        downstartwait = false;
    }
}
