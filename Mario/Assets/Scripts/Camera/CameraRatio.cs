using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatio : MonoBehaviour
{
    public Vector2 targetaspects = new Vector2(15, 15);
    //Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        float targetaspect = targetaspects.x / targetaspects.y;
        float windowaspect = Screen.width * 1.0f / Screen.height;
        float scaleheight = windowaspect / targetaspect;
        Camera camera = GetComponent<Camera>();
        if(scaleheight<1)
        {
            Rect rect = camera.rect;
            rect.height = scaleheight;
            rect.width = 1;
            rect.x = 0;
            rect.y = (1 - scaleheight) / 2f;
            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1f / scaleheight;
            Rect rect = camera.rect;
            rect.width = scalewidth;
            rect.height = 1;
            rect.x = (1 - scalewidth) / 2f;
            rect.y = 0;
            camera.rect = rect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
