using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Globalization;

public class AutonomousControl : MonoBehaviour
{
    public string filename_pos = "D:\\Data_Torino_experiments\\2021_07_07_MI02\\Pos_MI02_train_2_joystick_CT.txt";
    public string filename_ori = "D:\\Data_Torino_experiments\\2021_07_07_MI02\\Ori_MI02_train_2_joystick_CT.txt";
    public string filename_polyp = "D:\\Data_Torino_experiments\\2021_07_07_MI02\\Pol_MI02_train_2_joystick_CT.txt";
    Vector3[] pos = new Vector3[100000];
    Vector3[] ori = new Vector3[100000];
    public int[] polyp;
    public int index_pos = 0;
    public int index_ori = 0;
    public int index_pol = 0;
    float imAFloat = 0.000001f;
    public int index_update = 0;
    private ActivateCircle circle_script;
    bool turnOnCircle = false;
    
    void Start()
    {
        // polyp = new int[50000];
        // circle_script = GameObject.Find("Circle canvas").GetComponent<ActivateCircle>();

        ReadPos();
        ReadOri();
        // ReadPolyp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(index_update < index_pos)
        {
            transform.position = pos[index_update];
            //Debug.Log("here is my position"+pos[index_update]);
            Quaternion rotation = Quaternion.Euler(ori[index_update]);
            // transform.rotation = transform.rotation*rotation ;
             transform.rotation = rotation ;

            // if (polyp[index_update]== 1 && turnOnCircle == false)
            // {
            //     circle_script.TurnOnCircle();
            //     turnOnCircle = true;
            // }
            // else if (polyp[index_update]== 2)
            // {
            //     circle_script.CaptureCircle();
            // }
            // else if (polyp[index_update] == 0 && turnOnCircle == true)
            // {
            //     circle_script.TurnOnCircle();
            //     turnOnCircle = false;
            // }

            index_update = index_update + 1;
        }
        
        //Debug.Log("polyp"+ polyp[index_update]);

    }

    void ReadPos()
    {
        
        StreamReader strReader = new StreamReader(filename_pos);
        bool endOfFile = false;

        while (!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_string.Split(' ');

            pos[index_pos].x = float.Parse(data_values[0]);
            pos[index_pos].y = float.Parse(data_values[1]);
            pos[index_pos].z = float.Parse(data_values[2]);
            //Debug.Log("data values"+data_values[0]);
            //Debug.Log("pos index"+pos[index_pos].y);
            index_pos = index_pos + 1; 

        }
        strReader.Close();

    }

    void ReadOri()
    {
        
        StreamReader strReader = new StreamReader(filename_ori);
        bool endOfFile = false;

        while (!endOfFile)
        {
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_string.Split(' ');

            ori[index_ori].x = float.Parse(data_values[0]);
            ori[index_ori].y = float.Parse(data_values[1]);
            ori[index_ori].z = float.Parse(data_values[2]);

            index_ori = index_ori + 1; 

        }
        strReader.Close();
    }
    // void ReadPolyp()
    // {
        
    //     StreamReader strReader = new StreamReader(filename_polyp);
    //     bool endOfFile = false;

    //     while (!endOfFile)
    //     {
    //         string data_string = strReader.ReadLine();
    //         if (data_string == null)
    //         {
    //             endOfFile = true;
    //             break;
    //         }
    //         char first = data_string[0];
    //         // polyp[index_pol] = (int)Char.GetNumericValue(first);
    //         index_pol = index_pol + 1; 
    //     }
    //     strReader.Close();

    // }

}
