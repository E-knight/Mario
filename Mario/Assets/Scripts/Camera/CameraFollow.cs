using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //public float smoothdamptime = 0.15f;
    //private float smoothdampspeed = 0;
    public Transform target;
    //leftbounds,rightbounds;
    //private float camerawidth, cameraheight;
    public float levelminx, levelmaxx;
    // Start is called before the first frame update
    void Start()
    {
        //cameraheight = Camera.main.orthographicSize * 2;
       // camerawidth = cameraheight * Camera.main.aspect;
        //float leftboundwidth = leftbounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        //float rightboundwidth = rightbounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
       // levelminx = leftbounds.position.x + leftboundwidth + camerawidth / 2;
       // levelmaxx = rightbounds.position.x - rightboundwidth - camerawidth / 2;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(target)
        {
            float targetx = Mathf.Max(levelminx, Mathf.Min(levelmaxx, target.position.x));
            float x = Mathf.SmoothDamp(transform.position.x, targetx, ref smoothdampspeed, smoothdamptime);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }*/
        Vector3 v = transform.position;
        v.x = target.position.x;
        if (v.x > levelmaxx)
            v.x = levelmaxx;
        else if (v.x < levelminx)
            v.x = levelminx;
        transform.position = v;
            
    }
}
