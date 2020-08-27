using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Transform leftboundary, rightboundary;
    float camerawidth;
    Vector3 targetposition,reviveposition;
    public GameObject target;
    public float smooth = 5, followahead = 2.6f;
    public bool canmove, movebackward = false;
    Mario mario;
    //internal Rect rect;

    // Start is called before the first frame update
    void Start()
    {
        mario = FindObjectOfType<Mario>();
        target = mario.gameObject;
        GameObject boundary = GameObject.Find("Level Boundary");
        leftboundary = boundary.transform.Find("Left Boundary").transform;
        rightboundary = boundary.transform.Find("Right Boundary").transform;
        reviveposition = FindObjectOfType<LevelManager>().FindReveviePoint();
        targetposition = reviveposition;
        CameraRatio cameraRatio = GetComponent<CameraRatio>();
        float aspectratio = cameraRatio.targetaspects.x / cameraRatio.targetaspects.y;
        camerawidth = Camera.main.orthographicSize * aspectratio;
        bool passleftboundary = false;
        if (targetposition.x < leftboundary.position.x + camerawidth)
            passleftboundary = true;
        if(2*camerawidth>=rightboundary.position.x-leftboundary.position.x)
        {
            canmove = false;
            transform.position = new Vector3((leftboundary.position.x + rightboundary.position.x) / 2f, targetposition.y, targetposition.z);

        }
        else if(passleftboundary)
        {
            canmove = true;
            transform.position = new Vector3(leftboundary.position.x + camerawidth, targetposition.y, targetposition.z);
        }
        else
        {
            canmove = true;
            transform.position = new Vector3(targetposition.x + followahead, targetposition.y, targetposition.z);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canmove)
        {
            bool passleftboundary = false;
            bool passrightboundary = false;
            if (transform.position.x < leftboundary.position.x + camerawidth)
                passleftboundary = true;
            if (transform.position.x > rightboundary.position.x - camerawidth)
                passrightboundary = true;
            targetposition = transform.position;
            if (target.transform.localScale.x > 0 && !passrightboundary && targetposition.x - leftboundary.position.x >= camerawidth - followahead)
            {
                if (movebackward || target.transform.position.x + followahead >= transform.position.x)
                {
                    targetposition.x += followahead;
                    transform.position = Vector3.Lerp(transform.position, targetposition, smooth * Time.deltaTime);
                }


            }
            else if (target.transform.localScale.x<0&&movebackward&&!passleftboundary&&rightboundary.position.x-targetposition.x>=camerawidth-followahead)
            {
                targetposition.x -= followahead;
                transform.position = Vector3.Lerp(transform.position, targetposition, smooth * Time.deltaTime);
            }
        }
    }
}
