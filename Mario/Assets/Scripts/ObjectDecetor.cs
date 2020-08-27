using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDecetor : MonoBehaviour
{
    Dictionary<GameObject, Transform> objects;
    // Start is called before the first frame update
    void Start()
    {
        objects = new Dictionary<GameObject, Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!objects.ContainsKey(collision.gameObject))
        {
            objects.Add(collision.gameObject, collision.transform.parent);
            collision.transform.parent = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Transform oldparent;
        if(objects.TryGetValue(collision.gameObject,out oldparent))
        {
            collision.transform.parent = oldparent;
            objects.Remove(collision.gameObject);
        }
    }
}
