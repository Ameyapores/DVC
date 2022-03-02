//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using SofaUnity;
//using System;
//using System.IO;
//using System.Globalization;
//using System.Linq;
////using Newtonsoft.Json;

//[System.Serializable]
//public class arraychangeMesh
//    {
//        public changeMesh[] diffMesharray;
//        //int sadfja;
//    }

//[System.Serializable]
//public struct changeMesh
//    {
//        public int index;
//        public Vector3 Pos;
//        public float dist;
//    }

//public class GetChangeMesh_2 : MonoBehaviour
//{
//    [SerializeField] GameObject meshobject; 

//    protected Vector3[] vertexBuffer;
//    private Vector3[] vertices_1;
//    private Vector3[] vertices_2;

//    private arraychangeMesh new_array;
//    [SerializeField]
//    protected changeMesh[] diffMesh;

//    //protected string FileName = null;
//    int deltatime=0;
//    public float threshold = 40.0f;
//    //private Martina3_GameObjectController controller_handle;
    
//    //values To send to the controllers
//    public bool enable_movement = true;
//    public Vector3 movement_vector;
//    public GameObject capsule;

//    public bool deformation = false;
//    public Vector3 movement_vector_0;

//    public float[] distances_0 = new float[10000];
//    public int[] indices_0 = new int[10000];

//    void Start()
//    {

//        Mesh mesh = meshobject.GetComponent<MeshFilter>().sharedMesh;
//        vertices_1 = mesh.vertices;

//        capsule = GameObject.Find("Capsule");

//        //controller_handle = GameObject.Find("Capsule").GetComponent<Martina3_GameObjectController>();
//        //new_array = new arraychangeMesh();
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        deltatime=deltatime + 1;
//        if (deltatime==50){ //every second

//            Mesh mesh = meshobject.GetComponent<MeshFilter>().sharedMesh;
//            vertices_2 = mesh.vertices;

//            diffMesh = new changeMesh[vertices_2.Length];
//            //new_array.diffMesharray = new changeMesh[vertices_2.Length];
//            int j = 0;
//            int maxdist = 0;
//            float[] distances = new float[2000];
//            Vector3[] movement_vectors = new Vector3[2000];

//            //Vector3[] movement_vectors_0 = new Vector3[10000];
//            //float[] distances_0 = new float[10000];

//            for (int i = 0; i < vertices_2.Length; i++)
//            {
//                float distance = Vector3.Distance(vertices_2[i], vertices_1[i]);
//                if (distance >1.0f) //1.0f
//                {
//                    Vector3 vector_dist = vertices_2[i] -vertices_1[i];
//                    diffMesh[j].index=i;
//                    diffMesh[j].dist=distance;

//                    // deformed mesh
//                    // movement_vectors_0[j] = vector_dist;
//                    // distances_0[j] = distance;
//                    // indices_0[j] = i;
//                    j =j+1;


//                    if (distance > threshold)
//                    {
//                        Vector3 capsule_pos= capsule.transform.position;
//                        float dist = Vector3.Distance(capsule_pos, vertices_2[i]);

//                        if (dist  <  threshold) 
//                        {
//                            //Debug.Log("Entered in dist" + dist);
//                            distances[maxdist] = distance;
//                            movement_vectors[maxdist] = vector_dist;
//                            maxdist = maxdist + 1;
//                        }
//                    }
                    
//                }
            
//            }

//            if (maxdist == 0)
//            {
//                enable_movement = true;

//            }
//            else
//            {
//                int MaxIndex;
//                int MaxArrayValue = GetMaxArrayElement(distances, out MaxIndex);
//                movement_vector = movement_vectors[MaxIndex];
//                //Debug.Log("movement_vector" + movement_vector);
//                enable_movement = false;
//            }

//            // if (j != 0)
//            // {
//            //     int MaxIndex;
//            //     int MaxArrayValue = GetMaxArrayElement(distances_0, out MaxIndex);
//            //     movement_vector_0 = movement_vectors_0[MaxIndex];

//            //     if (!deformation)
//            //     {
//            //         // Debug.Log("j "+ j);
//            //         // Debug.Log("distance max " + distances_0[MaxIndex]);
//            //         // Debug.Log("indices " + indices_0);

//            //     }

//            //     deformation = true;
//            // }
//            // else deformation = false;

//            deltatime = 0;
//        }

//    }
            

//    public string getmeshdiff()
//    {
//        return JsonHelper.ToJson(diffMesh);
//    }
    
//    public int GetMaxArrayElement (float[] array, out int index) 
//    {
//        index = -1;
//        float max = float.MinValue;
//        for (int i = 0; i < array.Length; ++i) 
//        {
//            if (array[i] > max) 
//            {
//                max = array[i];
//                index = i;
//            }
//        }
//        return index;
//    }
//}

//public static class JsonHelper
//{
//    public static T[] FromJson<T>(string json)
//    {
//        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//        return wrapper.Items;
//    }

//    public static string ToJson<T>(T[] array)
//    {
//        Wrapper<T> wrapper = new Wrapper<T>();
//        wrapper.Items = array;
//        return JsonUtility.ToJson(wrapper);
//    }

//    public static string ToJson<T>(T[] array, bool prettyPrint)
//    {
//        Wrapper<T> wrapper = new Wrapper<T>();
//        wrapper.Items = array;
//        return JsonUtility.ToJson(wrapper, prettyPrint);
//    }

//    [Serializable]
//    private class Wrapper<T>
//    {
//        public T[] Items;
//    }
//}