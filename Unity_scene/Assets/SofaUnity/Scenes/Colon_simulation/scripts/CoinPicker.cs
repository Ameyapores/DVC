using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Globalization;

public class CoinPicker : MonoBehaviour
{

    private float coin = 0;
    protected Vector3 Pos = new Vector3(0.0f, 0.0f, 0.0f);
    protected Quaternion Ori = new Quaternion();
    protected string FileName = null;
    public bool CC_reached=false;

    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other) 
    {

        // if (other.tag == "CC"){
        //     other.gameObject.active = false;
        CC_reached = true;
        // }
    }
}
