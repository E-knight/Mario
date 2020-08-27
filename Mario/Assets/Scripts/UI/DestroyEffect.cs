using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float duration;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.gameObject)
            Destroy(gameObject.transform.parent.gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + duration);
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + duration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
