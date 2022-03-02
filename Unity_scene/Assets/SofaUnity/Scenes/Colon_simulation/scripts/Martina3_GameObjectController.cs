using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SofaUnity;

public class Martina3_GameObjectController : ObjectController
{
    /// Pointer to the current Mesh of the GameObject

    public bool m_isactive = false;
    public float t_factor = 20.0f;
    public float r_factor = 1f;

    public bool enable_movement = true;

    Vector3 ref_position;
    Quaternion ref_quaternion;
    protected GetChangeMesh_2 getMesh;

    protected Lines centerline;

    public CoinPicker ccReached;

    Vector3 position_0;

    public float angle_x = 0.0f;
    public float angle_y = 0.0f;
    public float angle_z = 0.0f;

    void Start()
    {

    ref_position = transform.position;
    ref_quaternion = transform.rotation;

    position_0 = transform.localEulerAngles;

    getMesh = GameObject.Find("OglModel  -  ColonVisualModel").GetComponent<GetChangeMesh_2>();
    centerline = GameObject.Find("Line").GetComponent<Lines>();
    ccReached = this.GetComponent<CoinPicker>();

    }

    

    void FixedUpdate()
    {
        if (!m_isactive)

            return;


        //ref_position = transform.position;
        //ref_quaternion = transform.rotation;

        // Prendo gli input e attualizzo la posizione di reference
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if ((Input.GetKey(KeyCode.J))|| (Input.GetKey(KeyCode.Keypad4)))
            {
                Quaternion pos = transform.rotation;
                Quaternion rot = Quaternion.Euler(0, 0, -r_factor);
               

                if (angle_z > -135.0f)
                {
                    angle_z = angle_z - r_factor;
                    ref_quaternion = pos * rot;
                }
                

            }
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 0.5f, transform.eulerAngles.z);
            else if ((Input.GetKey(KeyCode.L)) || (Input.GetKey(KeyCode.Keypad6)))
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 0.5f, transform.eulerAngles.z);
            {
                Quaternion pos = transform.rotation;
                Quaternion rot = Quaternion.Euler(0, 0, r_factor);

                 
                if (angle_z < 135.0f)
                {
                    angle_z = angle_z + r_factor;
                    ref_quaternion = pos * rot;
                }

            }
            else if ((Input.GetKey(KeyCode.I)) || (Input.GetKey(KeyCode.Keypad8)))
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x + 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
            {
                Quaternion pos = transform.rotation;
                Quaternion rot = Quaternion.Euler(-r_factor, 0, 0);
                if (angle_x > -135.0f)
                {
                    angle_x = angle_x - r_factor;
                    ref_quaternion = pos * rot;
                }
            }
            else if ((Input.GetKey(KeyCode.M)) || (Input.GetKey(KeyCode.Keypad2)))
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x - 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
            {
                Quaternion pos = transform.rotation;
                Quaternion rot = Quaternion.Euler(r_factor, 0, 0);
                if (angle_x < 135.0f)
                {
                    angle_x = angle_x + r_factor;
                    ref_quaternion = pos * rot;
                }
            }
            else if ((Input.GetKey(KeyCode.V)) || (Input.GetKey(KeyCode.Keypad7)))
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x + 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
            {
                Quaternion pos = transform.rotation;
                Quaternion rot = Quaternion.Euler(0, r_factor, 0);
                if (angle_y <  135.0f)
                {
                    angle_y = angle_y + r_factor;
                    ref_quaternion = pos * rot;
                }
            }
            else if ((Input.GetKey(KeyCode.N)) || (Input.GetKey(KeyCode.Keypad9)))
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x - 0.5f, transform.eulerAngles.y, transform.eulerAngles.z);
            {
                Quaternion pos = transform.rotation;
                Quaternion rot = Quaternion.Euler(0, -r_factor, 0);

                if (angle_y  > -135.0f)
                {
                    angle_y = angle_y - r_factor;
                    ref_quaternion = pos * rot;
                }

            }
            else if ((Input.GetKey(KeyCode.K))|| (Input.GetKey(KeyCode.Keypad5)))
            {
                ref_position = transform.position + transform.up * t_factor * Time.deltaTime;
                if (ccReached.CC_reached)
                {
                    //ref_quaternion=centerline.CorrectLine(25);
                    //Debug.Log("capsule " + transform.position + "ref_position " + ref_position);
                    Quaternion pos = transform.rotation;
                    Quaternion rot = Quaternion.identity;
                    //ref_quaternion = pos;
                    centerline.CorrectLine(15.0f, pos, ref ref_position, ref rot);
                    ref_quaternion =  rot * pos;
                }

            }
        }
        else
        {
            if ((Input.GetKey(KeyCode.I)) || (Input.GetKey(KeyCode.Keypad8)))
                ref_position = transform.position + transform.forward * t_factor * Time.deltaTime;
            else if ((Input.GetKey(KeyCode.M)) || (Input.GetKey(KeyCode.Keypad2)))
                ref_position = transform.position - transform.forward * t_factor * Time.deltaTime;
            else if ((Input.GetKey(KeyCode.J)) || (Input.GetKey(KeyCode.Keypad4)))
                ref_position = transform.position - transform.right * t_factor * Time.deltaTime;
            else if ((Input.GetKey(KeyCode.L)) || (Input.GetKey(KeyCode.Keypad6)))
                ref_position = transform.position + transform.right * t_factor * Time.deltaTime;
            else if ((Input.GetKey(KeyCode.K)) || (Input.GetKey(KeyCode.Keypad5)))
                ref_position = transform.position - transform.up * t_factor * Time.deltaTime;
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                t_factor += 1f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (t_factor > 2f)
                    t_factor -= 1f;
            }
        }

        
        SetPosition();
        transform.rotation = ref_quaternion;
        Debug.Log(transform.localEulerAngles);

        //Debug.Log("transform.localEulerAngles.x " + transform.localEulerAngles.x);

        //Debug.Log(ref_position);

    }

    public bool isToolActive()
    {
        return m_isactive;
    }

    void SetPosition(){
        if (getMesh.enable_movement)
        {
            transform.position = ref_position;
        }
        else 
        {
            Vector3 ref_vector = ref_position-transform.position;
            //Debug.Log("Mesh_def" + getMesh.movement_vector);
            float product = (Vector3.Dot(ref_vector, getMesh.movement_vector));
            

            if (product < 0){
                transform.position = ref_position;
            }

        }
    }

}

