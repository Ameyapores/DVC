using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject line1;
    public GameObject line2;
    public GameObject line3;
    public GameObject line4;

    // Start is called before the first frame update
    void Start()
    {
        line1 = GameObject.Find("Line (0)");
        line2 = GameObject.Find("Line (2)");
        line3 = GameObject.Find("Line (3)");
        line4 = GameObject.Find("Line (1)");


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            line1.active = !line1.active;
            line2.active = !line2.active;
            line3.active = !line3.active;
            line4.active = !line4.active;
            Debug.Log("Change");
        }
    }
}
