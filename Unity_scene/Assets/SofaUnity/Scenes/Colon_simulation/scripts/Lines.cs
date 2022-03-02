using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class Lines : MonoBehaviour
{
    LineRenderer line;

    public int index = 0;
    Vector3[] centerline = new Vector3[2000];
    float closest = 0.0f;
    Vector3 capsule_pos;
    float distance_up;
    float distance_down;
    float distance_downdown;
    float distance_upup;
    int i = 1;
    int old_centerline_index = 0;
    int i_closest = 0;

    public GameObject capsule;
    public GameObject capsule_obj;

    bool debug= false;
    Vector3 relativePos ;
    public float[] teta = new float[2000];

    public GameObject cup;
    public GameObject cdown;
    public GameObject c;
    public string filename;
    float imAFloat = 0.000001f;

    bool Endohadle = false;

    public int centerline_index;
    // public GameObject TheEnd;

    void Start()
    {
        ReadCSV();

        line = GetComponent<LineRenderer>();
        line.enabled = false;
        line.useWorldSpace = true;
        line.positionCount = index; //lenght of centerline


        // capsule = GameObject.Find("Capsule");

        // capsule_obj = GameObject.Find("CapsuleObj");
        // TheEnd = GameObject.Find("TheEnd");
        // TheEnd.active = false;
        // cup = GameObject.Find("cUP");
        // cdown = GameObject.Find("cDOWN");
        // c = GameObject.Find("cCENTER");

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("capsule_pos: " + capsule.transform.position);

        TrackLine(capsule.transform.position);
        PrintLine();


        //if (debug) Debug.DrawLine(relativePos, capsule.transform.position, Color.white, 50f);

    }

    void ReadCSV()
    {
        //StreamReader strReader = new StreamReader("C:\\Users\\marti\\SOfaUnity_test\\New Unity Project\\Assets\\SofaUnity\\Scenes\\Colon_simulation\\Demo_v2\\Patient_1\\centerline_csv3.csv");
        StreamReader strReader = new StreamReader(filename);
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

            centerline[index].x = float.Parse(data_values[0]);
            centerline[index].y = float.Parse(data_values[1]);
            centerline[index].z = float.Parse(data_values[2]);

            // Debug.Log("centerline" + centerline[index]);

            index = index + 1; //Puedo poner un comentario que no me pasa nada
            //Debug.Log(data_values[0].ToString() + " " + data_values[1].ToString() + " " + data_values[2].ToString());



            //Debug Centerline and save it in txt file
            // for (i = 0; i < index -5; i++)
            // {
            //     Vector3 c1 = centerline[index - i -6];
            //     Vector3 c2 = centerline[index - i - 5];
            //     Vector3 c3 = centerline[index - i - 4];
            //     Vector3 c4 = centerline[index - i - 3];
            //     Vector3 c5 = centerline[index - i - 2];
            //     Vector3 c6 = centerline[index - i - 1];



            //     Vector3 v = c5 - c1;
            //     Vector3 w = c6 -c2;
            //     float magnitude_v = v.magnitude;
            //     float magnitude_w = w.magnitude;

            //     teta[i] = Mathf.Acos(Vector3.Dot(v, w)/(magnitude_v * magnitude_w));

            //     //Save("C:\\Users\\marti\\SOfaUnity_test\\New Unity Project\\Assets\\SofaUnity\\Scenes\\Colon_simulation\\Demo_v2\\Patient_1\\angles3.txt", teta[i]);

            // }

            // using(StreamWriter sr = new StreamWriter("C:\\Users\\marti\\SOfaUnity_test\\New Unity Project\\Assets\\SofaUnity\\Scenes\\Colon_simulation\\Demo_v2\\Patient_1\\angles6.txt"))
            // {
            //     foreach(var item in teta)
            //     {
            //         sr.WriteLine(item);
            //     }
            // }


        }

    }

    void Save (string fileName, float time)
    {
        FileStream sb = new FileStream(fileName, FileMode.Append);

        using (StreamWriter sw = new StreamWriter(sb))
        {

            sw.Write(time + "\n");

        }
    }


    void PrintLine()
    {
        for (int i = 0; i < index; i++)
        {
            line.SetPosition(i, centerline[i]);
        }

    }

    void TrackLine(Vector3 capsule_pos)
    {
        closest = Vector3.Distance(capsule_pos, centerline[index - i]); // index is the length of the centerline
        distance_up = Vector3.Distance(capsule_pos, centerline[index - i - 1]);
        distance_down = Vector3.Distance(capsule_pos, centerline[index - i + 1]);

        distance_upup = Vector3.Distance(capsule_pos, centerline[index - i - 2]);
        distance_downdown = Vector3.Distance(capsule_pos, centerline[index - i + 2]);

        //Debug of the centerline with the capsules
        // cup.transform.position = centerline[index - i - 1];
        // cdown.transform.position = centerline[index - i + 1];
        // c.transform.position = centerline[index - i];


        // Debug.Log("distance_up: " + distance_up + "distance_down: " + distance_down + "Pos_up: " + centerline[index - i] + "Pos_down: " + centerline[index - i + 1]);
        // Debug.Log("distance_up " + distance_up);

        while (((distance_up < closest) || (distance_upup < closest)) && (index-i)>5)
        {
            i++;

            //Debug.Log("i+: " + i + "distance+: " + distance_up + "closest " + closest);
            closest = Vector3.Distance(capsule_pos, centerline[index - i]);
            distance_up = Vector3.Distance(capsule_pos, centerline[index - i - 1]);
            distance_upup = Vector3.Distance(capsule_pos, centerline[index - i - 2]);


        }

        while (((distance_down < closest) || (distance_downdown < closest)) && i>4)
        {
            i--;
            //Debug.Log("i-: " + i + "distance-: " + distance_down + "closest " + closest);
            closest = Vector3.Distance(capsule_pos, centerline[index - i]);
            distance_down = Vector3.Distance(capsule_pos, centerline[index - i + 1]);
            distance_downdown = Vector3.Distance(capsule_pos, centerline[index - i + 2]);

        }

        //Debug.Log("printing centerline index" + i);
        centerline_index = i;
        //Debug.Log("printing centerline index" + centerline_index);
       // Debug.Log("i " + i + "index - i" + (index - i));

    }

    public int Get_i_value()
    {
        int index_difference = centerline_index - old_centerline_index;
        // bool direction_reverse = false;
        
        // if (old_centerline_index > centerline_index) {direction_reverse = true;}
        // if (old_centerline_index < centerline_index) {direction_reverse =false;}
        //Debug.Log("printing old_ceterline" + (old_centerline_index, centerline_index));
        old_centerline_index = centerline_index;
        
        // Debug.Log("printing centerline index" + index_difference);
        return index_difference;
    }

    public void CorrectLine(float threshold, Quaternion rotation, ref Vector3 capsule_pos_ref, ref Quaternion capsule_rot_ref)
    {
        //Debug.Log("capsule_pos_ref" +  capsule_pos_ref);
        capsule_obj.transform.position = capsule_pos_ref;
        capsule_obj.transform.rotation = rotation;
        Vector3 up = capsule_obj.transform.up;
        Vector3 up_norm = Vector3.Normalize(up);

        //Debug.Log("up " + up);

        Vector3 back = new Vector3(0.0f, 5.0f, 0.0f);
        Vector3 P = capsule_obj.transform.position + capsule_obj.transform.up*9.0f; //back

        Vector3 c2 = centerline[index - i + 2];
        Vector3 c1 = centerline[index - i - 2];

        Vector3 v = c2 - c1;

        Vector3 v_norm = Vector3.Normalize(v);
        Vector3 w = P - c1;

        float lambda = Vector3.Dot(w, v_norm);
        Vector3 cp = c1 + lambda * v_norm;

        float distance = Vector3.Distance(P, cp);

        //Rotations
        float magnitude_v = v.magnitude;
        float magnitude_up = up.magnitude;

        float teta = Mathf.Acos(Vector3.Dot(v, up)/(magnitude_v * magnitude_up));
       // Debug.Log("teta " + teta);


        if (distance > threshold)
        {
            Vector3 P1 = cp + (Vector3.Normalize(P - cp) * threshold);
            capsule_pos_ref = P1 - capsule_obj.transform.up * 9.0f;
            //Debug.DrawLine(capsule_obj.transform.position, capsule_pos_ref, Color.blue, 40f);

        }

         if (teta > 0.7f)
        {
                //Debug.Log("tetaMax " + teta);
                Vector3 axis = Vector3.Cross(v,up);
                //Debug.DrawLine(capsule_obj.transform.position, capsule_obj.transform.position + axis*20, Color.white, 10f);

                capsule_rot_ref = Quaternion.AngleAxis(-1, Vector3.Normalize(axis));
        }
          

    }

    public void EndOfGame()
    {
        // if (i < 20)
        //     TheEnd.active = true;

    }





}
