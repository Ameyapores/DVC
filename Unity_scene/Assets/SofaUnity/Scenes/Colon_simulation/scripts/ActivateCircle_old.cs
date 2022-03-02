using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Globalization;

public class ActivateCircle_old : MonoBehaviour
{
    private CircleGraphic circle;
    public string filePath="C:/Users/marti/SOfaUnity_test/New Unity Project/Assets/SofaUnity/Scenes/Colon_simulation/Saved Files/";
    public string fileName = "Polyp_picker1.txt";
    protected string FileName = null;
    public GameObject capsule;

    void SavePolyp (string fileName, string time, Vector3 pos, Quaternion ori)
    {
        FileStream sb = new FileStream(fileName, FileMode.Append);

        using (StreamWriter sw = new StreamWriter(sb))
        {

            sw.Write(time + "\t" + pos + "\t" + ori +"\n");
             
            }
        }
    
    
    void Start ()
    {
        capsule = GameObject.Find("Capsule");
        circle = GetComponent<CircleGraphic>();
        circle.enabled = false;
        FileName = filePath + fileName;
    }
    
    
    void FixedUpdate ()
    {
        if(Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.C))
        {
            TurnOnCircle();
        }

        if (circle.enabled && (Input.GetKeyUp(KeyCode.X)||Input.GetKeyUp(KeyCode.B)))
        {
            CaptureCircle();
        }

        //Add other input to save the event
    }

    public void TurnOnCircle()
    {
        
        if (circle.enabled) 
        {
            circle.enabled = false;
            Debug.LogWarning("Deactive circle");
        }
        else 
        {
            circle.enabled = true;
            Debug.LogWarning("Active circle");
        }
        

        
        // Here send time stamp to lsl the time stamp + position and orientation of the capsule
    }
    public void CaptureCircle()
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        Vector3 Pos = capsule.transform.position;
        Quaternion Ori = capsule.transform.rotation;
        SavePolyp (FileName, timestamp, Pos, Ori);
        // Here send to lsl the position and orientation of the capsule
        Debug.LogWarning("Polyp saved");
    }
}