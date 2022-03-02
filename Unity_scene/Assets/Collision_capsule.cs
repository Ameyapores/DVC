using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision_capsule : MonoBehaviour
{
    public bool collided = false;
    // Start is called before the first frame update
    void Start()
    {
        collided = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Contacts")
        {
            collided = true;
            Debug.Log("Collided");
        }
    }
}
