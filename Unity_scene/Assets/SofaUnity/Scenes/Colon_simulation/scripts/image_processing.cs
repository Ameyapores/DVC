using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Globalization;
using System;
using System.Drawing;
using System.Threading.Tasks;
[RequireComponent(typeof(Camera))]

public class image_processing : MonoBehaviour
{
    private Camera _camera;
    //public GetChangeMesh_3 m_collision2;
    private IEnumerator coroutine;

    [SerializeField] GameObject centroidobject;
    // [SerializeField] GameObject text_render;
    private RectTransform centroid;
    private CircleGraphic circle;
    // private Text image_text;
    private int count;
    private float distance;

    private float centroid_x_value;
    private float centroid_y_value;    

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
        centroid= centroidobject.GetComponent<RectTransform>();
        circle= centroidobject.GetComponent<CircleGraphic>();
        // image_text = text_render.GetComponent<Text>();
        circle.enabled = true;
        // image_text.enabled = false;
        //loggingEnabled = true;
        
        // coroutine = Logging();
        // StartCoroutine(coroutine);
    }

    void Update() 
    {
        StartCoroutine(detectcentroid()) ;   
    }

    // Update is called once per frame
    public IEnumerator detectcentroid()
    {   
        //Capture();
        RenderTexture activeRenderTexture = RenderTexture.active;
        //Debug.Log(_camera);
        RenderTexture.active = _camera.targetTexture;

        _camera.Render();

        Texture2D image = new Texture2D(_camera.targetTexture.width,_camera.targetTexture.height);

        image.ReadPixels(new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height), 0, 0);
        image.Apply();

        // Color[] pixels = image.GetPixels();
        // Color[] pixels_back = new Color[pixels.Length];
        // // Color bkac

        //Debug.Log(pixels.Length);
        // Texture2D image_cpy = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);
        // image_cpy.SetPixels(image.GetPixels());
        // image_cpy.Apply();
        // Graphics.CopyTexture(image, image_cpy);
        //float orig_gray = 0;
        RenderTexture.active = activeRenderTexture;
        count = 0;
        int centroid_x = 0; 
        int centroid_y = 0;
        int width =  _camera.targetTexture.width;
        int height =  _camera.targetTexture.height;
        Texture2D graytex = new Texture2D(_camera.targetTexture.width, _camera.targetTexture.height);

        for (int i=0; i < width; i++)
        {
            for (int j=0; j < height; j++)
            {
                if((i-width/2)*(i-width/2)+(j-height/2)*(j-height/2) < 42*42)
                {
                    Color originalColor = image.GetPixel(i, j);

                    float grayScale = ((originalColor.r * .3f) + (originalColor.g * .59f) + (originalColor.b * .11f));

                    if (grayScale<0.1)
                    {
                        graytex.SetPixel(i,j, Color.black);
                        // graytex.SetPixel(i,j, originalColor);
                        centroid_x += i;
                        centroid_y += j;
                        count++;
                    }
                    else
                    {
                        // graytex.SetPixel(i,j, originalColor);
                        graytex.SetPixel(i,j,Color.white);
                    }
                }
                else
                {
                    // graytex.SetPixel(i,j, originalColor);
                    graytex.SetPixel(i,j,Color.white);
                }
            }   
        }

        // Parallel.For(0, width, i => 
        // {
        //     for (int j=0; j < width; j++)
        //     {
        //         if((i-width/2)*(i-width/2)+(j-height/2)*(j-height/2) < 100*100)
        //         {
        //             // Color originalColor = image_cpy.GetPixel(i,j);
                    
        //             float grayScale = ((pixels[width * j + i].r * .3f) + (pixels[width * j + i].g * .59f) + (pixels[width * j + i].b * .11f));
                
        //             if (grayScale<0.2)
        //             {
        //                 // graytex.SetPixel(i,j, Color.black);
        //                 pixels_back[width * j + i] = Color.black;
        //                 centroid_x += i;
        //                 centroid_y += j;
        //                 count ++;
        //             }
        //             else
        //             {
        //                 // graytex.SetPixel(i,j,Color.white);
        //                 pixels_back[width * j + i]  = Color.white;
        //             }

        //         }
        //         else
        //         {
        //             pixels_back[width * j + i] = Color.white;
        //             // graytex.SetPixel(i,j,Color.white);
        //         }
        //     }
        // });

        // graytex.SetPixels(pixels_back);
        graytex.Apply();
        //Debug.Log("count" + count);
        if (count > 100)
            {   
                centroid_x /= count;
                centroid_y /= count;
                Vector3 centroid_vector = new Vector3(centroid_x, centroid_y, 0.0f);
                Vector3 image_center = new Vector3(width/2, height/2, 0.0f);
                centroid.transform.position = centroid_vector;
                distance = Vector3.Distance(centroid_vector, image_center);
                circle.enabled = true;
                // image_text.enabled = false;
                centroid_x_value = centroid_x/84;
                centroid_y_value = centroid_y/84;
            }
        else
        {
            circle.enabled = false;
            // image_text.enabled = true;
        }
        
        
        // if((i-centroid_x_value)*(i-centroid_x_value)+(j-centroid_y_value)*(j-centroid_y_value) <5*5)
        // {
        //     graytex.SetPixel(i,j, Color.green);
        // }
        

        // byte[] bytes = graytex.EncodeToPNG();
        Destroy(image);
        Destroy(graytex);

        //Debug.Log("Printing bytes"+bytes);
        // string timestamp = DateTime.Now.ToString("yyyyMMdd_HH_mm_ss_fff");

        // File.WriteAllBytes("G:/Amey/Surgeon_experiments/RL_data/segmentation" + "/segmented/" + "_" + timestamp +".png", bytes);
        
        yield return new WaitForEndOfFrame();
    }
    // public void Capture()
    // {
        
    // }

    public float Getdistance()
    {   
        float normalised_distance = 42f;
        if (count > 100)
        {   
            return distance/normalised_distance;
        }
        else
        {
            return 1.0f;
        }
            
    }

    public float Get_x_value()
    {
        return centroid_x_value;
    }

    public float Get_y_value()
    {
        return centroid_y_value;
    }
}
