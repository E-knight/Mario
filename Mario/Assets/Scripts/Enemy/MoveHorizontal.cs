using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{
    GameObject mario;
    public Transform left, right;
    public float speed,speedmodify=1,direction=1;
    public bool canmove = false, automove = true;
    float movedistance = 40,currentspeed;
    public float waitleft, waitright;
    public bool atleft, atright;
    bool waitleftstart, waitrightstart;
    // Start is called before the first frame update
    void Start()
    {
        mario = FindObjectOfType<Mario>().gameObject;
        if (transform.position.x >= right.position.x)
            direction = -1;
        else if (transform.position.x <= left.position.x)
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
            if(!atleft&&!atright)
            {
                currentspeed *= speedmodify;
                transform.position += new Vector3(currentspeed * direction, 0, 0);
                if (transform.position.x <= left.position.x)
                    atleft = true;
                else
                    atleft = false;
                if (transform.position.x >= right.position.x)
                    atright = true;
                else
                    atright = false;
            }
            else if(atleft&&!waitleftstart)
            {
                StartCoroutine(WaitAtLeftCoroutine());
                waitleftstart = true;
            }
            else if(atright&&!waitrightstart)
            {
                StartCoroutine(WaitAtRightCoroutine());
                waitrightstart = true;
            }
        }
    }

    IEnumerator WaitAtLeftCoroutine()
    {
        yield return new WaitForSeconds(waitleft);
        currentspeed = speed;
        direction = 1;
        atleft = false;
        waitleftstart = false;
    }

    IEnumerator WaitAtRightCoroutine()
    {
        yield return new WaitForSeconds(waitright);
        currentspeed = speed;
        direction = -1;
        atright = false;
        waitrightstart = false;
    }
}
