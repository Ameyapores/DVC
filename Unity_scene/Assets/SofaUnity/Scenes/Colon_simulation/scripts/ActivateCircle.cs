using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCircle : MonoBehaviour
{
    private float Sample;
    [SerializeField] GameObject Lines;

    bool turnoncircle = false;
    bool capturecircle = false;

    public GameObject Text;
 
    int counter = 0;

    void Start()
    {
        Lines = GameObject.Find("Lines");
        Text = GameObject.Find("Text");
        //TheEnd = GameObject.Find("TheEnd");

        Lines.active = false;
        Text.active = false;
 


    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            TurnOnCircle();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            CaptureCircle();
        }


        if (Text.active)
        {
            counter = counter + 1;
            if (counter > 10)
            {
                capturecircle = false;
            }
                if (counter > 60) //3 secondi
            {
                counter = 0;
                Text.active = false;
            }
        }
        
    }

    public void TurnOnCircle()
    {
        if (Lines.active == false)
        {
            Lines.active = true;
            turnoncircle = true;
        }
        else if (Lines.active == true)
        {
            Lines.active = false;
            turnoncircle = false;
        }

        if (turnoncircle == false)
        {
           // Debug.Log("No circle");
            capturecircle = false;
        }

    }

    public void CaptureCircle()
    {
        if (Lines.active)
        {
            capturecircle = true;
           // Debug.Log("Polyp saved");
            Text.active = true;
        }

    }

    public float Getcircleoutput()
    {
        if (turnoncircle)
        {
            if (capturecircle == true)
                Sample = 2;
            else
                Sample = 1;
        }

        else
        {
            Sample = 0;
        }
        return Sample;
    }



}