using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevivePoint : MonoBehaviour
{
    Mario mario;
    // Start is called before the first frame update
    void Start()
    {
        mario = FindObjectOfType<Mario>();

    }

    // Update is called once per frame
    void Update()
    {
        if(mario.gameObject.transform.position.x>=transform.position.x-1.1)
        {
            GameStateManager manager = FindObjectOfType<GameStateManager>();
            manager.revivepointx = Mathf.Max(manager.revivepointx, gameObject.transform.GetSiblingIndex());
            gameObject.SetActive(false);
        }
    }
}
