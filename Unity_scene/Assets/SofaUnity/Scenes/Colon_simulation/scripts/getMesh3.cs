using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;
using System;
using System.IO;
using System.Globalization;

public class getMesh3 : MonoBehaviour
{
    public string filePath = "C:/Users/marti/SOfaUnity_test/New Unity Project/Assets/SofaUnity/Scenes/Colon_simulation/Saved Files/";
    public string fileName = "MeshChange_2_1.txt";
    protected Vector3[] vertexBuffer;
    private Vector3[] vertices_1;
    private Vector3[] vertices_2;
    public struct changeMesh
    {
        public int index;
        public Vector3 Pos;
        public float dist;
    }
    protected changeMesh[] diffMesh;

    protected string FileName = null;
    int deltatime = 0;
    public float threshold = 40.0f;
    //private Martina2_GameObjectController controller_handle;

    //values To send to the controllers
    public bool enable_movement = true;
    public Vector3 movement_vector;
    public GameObject capsule;


    void Save_meshChange(string fileName, string time, int len, changeMesh[] buffer)
    {
        FileStream sb = new FileStream(fileName, FileMode.Append);

        using (StreamWriter sw = new StreamWriter(sb))
        {
            sw.Write(time + "\n");

            for (int i = 0; i < len; i++)
            {
                //sw.Write(buffer[i].index + "\t" + buffer[i].Pos + "\n"); write timestamp
                sw.Write(buffer[i].index + "\t" + buffer[i].Pos + "\n");

            }

            sw.Write("\n");


        }
    }


    void Start()
    {
        FileName = filePath + fileName;

        Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;
        vertices_1 = mesh.vertices;

        capsule = GameObject.Find("Capsule");

        //controller_handle = GameObject.Find("Capsule").GetComponent<Martina2_GameObjectController>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        deltatime = deltatime + 1;
        if (deltatime == 50)
        { //every second

            Mesh mesh = this.GetComponent<MeshFilter>().sharedMesh;
            vertices_2 = mesh.vertices;
            changeMesh[] diffMesh = new changeMesh[vertices_2.Length];
            int j = 0;
            int maxdist = 0;
            float[] distances = new float[2000];
            Vector3[] movement_vectors = new Vector3[2000];

            for (int i = 0; i < vertices_2.Length; i++)
            {
                float distance = Vector3.Distance(vertices_2[i], vertices_1[i]);
                if (distance > 1.0)
                {
                    Vector3 vector_dist = vertices_2[i] - vertices_1[i];
                    diffMesh[j].index = i;
                    diffMesh[j].dist = distance;
                    j = j + 1;

                    if (distance > threshold)
                    {
                        Vector3 capsule_pos = capsule.transform.position;
                        float dist = Vector3.Distance(capsule_pos, vertices_2[i]);
                        //Debug.Log("Distance" + dist);

                        //check if the deformation is directly caused by the capsule
                        if (dist < threshold)
                        {
                            //Debug.Log("Entered in dist" + dist);
                            distances[maxdist] = distance;
                            movement_vectors[maxdist] = vector_dist;
                            maxdist = maxdist + 1;
                        }
                        //Debug.Log("vector_dist" + vector_dist);
                        //Debug.Log("Distance element" + i + "reached threshold");
                        //get the vector diffMesh[j].Pos
                        //enable_movement = false;
                        //movement_vector = diffMesh[j].Pos;


                    }
                }

            }

            if (maxdist == 0)
            {
                enable_movement = true;

            }
            else
            {
                int MaxIndex;
                int MaxArrayValue = GetMaxArrayElement(distances, out MaxIndex);
                movement_vector = movement_vectors[MaxIndex];
                //Debug.Log("movement_vector" + movement_vector);
                enable_movement = false;
            }

            if (j != 0)
            {
                // Here send time lsl j e diffMesh



                //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                //Save_meshChange(FileName, timestamp, j, diffMesh);
                //Debug.Log("j final is:" + j);
            }

            //vertices_1=vertices_2;

            deltatime = 0;
        }

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
