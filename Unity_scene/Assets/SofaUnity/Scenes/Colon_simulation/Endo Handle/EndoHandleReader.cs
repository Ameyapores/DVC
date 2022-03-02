using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mimesis_INRIA.EVEREST;

public class EndoHandleReader : MonoBehaviour
{
    EndoHandle eh;

    void Start()
    {
        eh = GameObject.FindObjectOfType<EndoHandle>();
    }

    void Update()
    {
        if(eh != null)
        {
            Debug.Log(
                "rotangle1:" + eh.rotangle1 + "\n"+
                "rotangle2:" + eh.rotangle2 + "\n" +
                "Buttons:" + eh.Buttons + "\n" +
                "imuOrientation:" + eh.imuOrientation + "\n" );
        }
    }
}
