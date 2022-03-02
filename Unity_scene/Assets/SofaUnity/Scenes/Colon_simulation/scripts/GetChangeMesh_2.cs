using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;
using System.IO;
using System.Globalization;
using System.Linq;
using LSL;

// using System.Threading.Tasks;
//using Newtonsoft.Json;

[System.Serializable]
public struct changeMesh
{
    public int index;
    public Vector3 Pos;
    public float dist;
}

public class GetChangeMesh_2 : MonoBehaviour
{
    [SerializeField] GameObject meshobject;
    private StreamOutlet outlet2;
    public string StreamName2 = "Unity.ExampleStream2";
    public string StreamType2 = "Unity.StreamType2";
    public string StreamId2 = "MyStreamID-Unity12345";

    protected Vector3[] vertexBuffer;
    private Vector3[] vertices_1;
    private Vector3[] vertices_2;

    [SerializeField]
    protected changeMesh[] diffMesh;
    protected List<changeMesh> list;

    //protected string FileName = null;
    public float threshold = 40.0f;

    //values To send to the controllers
    public bool enable_movement = true;
    public Vector3 movement_vector;
    public GameObject capsule;

    private bool LoggingEnabled;
    private WaitForSecondsRealtime wait;
    private System.Diagnostics.Stopwatch stopwatch;
    private float count;

    private bool flag = false;

    void Start()
    {

        Mesh mesh = meshobject.GetComponent<MeshFilter>().sharedMesh;
        vertices_1 = mesh.vertices;

        capsule = GameObject.Find("Capsule");

        StreamInfo streamInfo2 = new StreamInfo(StreamName2, StreamType2, 1, 2, channel_format_t.cf_string);
        outlet2 = new StreamOutlet(streamInfo2);
        count = 0;
        wait = new WaitForSecondsRealtime(1.0f / 2);
        LoggingEnabled = true;
        IEnumerator logging = LogData();
        StartCoroutine(logging);
    }

    private IEnumerator LogData()
    {
        while (LoggingEnabled)
        {
            Mesh mesh = meshobject.GetComponent<MeshFilter>().sharedMesh;
            vertices_2 = mesh.vertices;
            list = new List<changeMesh>();

            int maxdist = 0;
            float[] distances = new float[2000];
            Vector3[] movement_vectors = new Vector3[2000];

            for (int i = 0; i < vertices_2.Length; i++)
            {
                float distance = Vector3.Distance(vertices_2[i], vertices_1[i]);
                if (distance > 1.0f) //1.0f
                {
                    Vector3 vector_dist = vertices_2[i] - vertices_1[i];
                    // Debug.Log("sjfkfs;ldkfl;d");
                    changeMesh tmp = new changeMesh();
                    tmp.dist = distance;
                    tmp.index = i;
                    tmp.Pos = vertices_2[i];
                    list.Add(tmp);

                    // if (distance > threshold)
                    // {
                    //     Vector3 capsule_pos = capsule.transform.position;
                    //     float dist = Vector3.Distance(capsule_pos, vertices_2[i]);

                    //     if (dist < threshold)
                    //     {
                    //         distances[maxdist] = distance;
                    //         movement_vectors[maxdist] = vector_dist;
                    //         maxdist = maxdist + 1;
                    //     }
                    // }

                }

            }
            
            diffMesh = list.ToArray();

            if (maxdist == 0)
            {
                enable_movement = true;
            }
            else
            {
                int MaxIndex;
                int MaxArrayValue = GetMaxArrayElement(distances, out MaxIndex);
                movement_vector = movement_vectors[MaxIndex];
                enable_movement = false;
            }

            string[] readText = new string[] { getmeshdiff() };
            //Debug.Log(readText);
            // System.IO.File.WriteAllText("G:/Amey/Surgeon_experiments/Data_analysis/mesh_pos/WriteText.txt",readText);
            // if (flag == false)
            // {
            //     using (StreamWriter sw = new StreamWriter("G:/Amey/Surgeon_experiments/Data_analysis/mesh_pos/WriteText.txt"))
            //     {
            //         sw.WriteLine(JsonHelper.ToJson(diffMesh));
            //         //Debug.Log("Hello");
            //         flag = true;
            //     }
            // }
            outlet2.push_sample(readText);
            count++;
            yield return wait;
        }

    }

    // public string getmeshdiff()
    // {
    //     //Debug.Log(diffMesh.Length);
    //     if (IsNullOrEmpty(diffMesh))
    //         return JsonHelper.ToJson(vertices_2);
    //         // return "Null";
    //     else
    //         return JsonHelper.ToJson(diffMesh);
    // }

    public string getmeshdiff()
    {
        //Debug.Log(diffMesh.Length);
        if (IsNullOrEmpty(diffMesh))
            return "Null";
        else
            return JsonHelper.ToJson(diffMesh);
    }

    public bool IsNullOrEmpty(Array array)
    {
        return (array == null || array.Length == 0);
    }

    public int GetMaxArrayElement(float[] array, out int index)
    {
        index = -1;
        float max = float.MinValue;
        for (int i = 0; i < array.Length; ++i)
        {
            if (array[i] > max)
            {
                max = array[i];
                index = i;
            }
        }
        return index;
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}