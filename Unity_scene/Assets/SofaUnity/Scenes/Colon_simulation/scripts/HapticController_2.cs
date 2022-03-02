using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticController_2 : MonoBehaviour
{
    private HapticPlugin haptic;
    public Vector3 positions;
    public Quaternion rotations;

    public Vector3 PosRef = new Vector3(0.0f, 0.0f, 0.0f);
    public float factor_t_prop = 20.0f;
    public float factor_t_button = 20.0f;
    public float factor_r = 1.35f;
    float res = 15.0f;
    bool start = false;
    public bool m_isactive = true;

    //GameObject circleobject;
    //private CircleGraphic circle;
    private ActivateCircle circle;
    bool flagButton_1 = false;
    bool flagButton_2 = false;
    bool flagStart = false;

    int timer_1 = 0;
    int timer_2 = 0;
    int timer_start = 0;
    public int CtlType = 0; //buttons control

    Vector3 ref_position;
    public bool enable_movement = true;
    protected GetChangeMesh_2 getMesh;

    protected Lines centerline;
    public CoinPicker ccReached;

    public float angle_x = 0.0f;
    public float angle_y = 0.0f;
    public float angle_z = 0.0f;

    public Vector3 deltaPos;

    void Start()
    {
        haptic = GameObject.Find("HapticDevice").GetComponent<HapticPlugin>();

        ref_position = transform.position;
        getMesh = GameObject.Find("OglModel  -  ColonVisualModel").GetComponent<GetChangeMesh_2>();

        centerline = GameObject.Find("Line").GetComponent<Lines>();
        ccReached = this.GetComponent<CoinPicker>();

        circle = GameObject.Find("Circle canvas").GetComponent<ActivateCircle>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        //To start using the haptic device click both the buttons
        if (!start)
        {
            if (haptic.Buttons[0] == 1 && haptic.Buttons[1] == 1)
            {
                flagStart = true;
            }

            if (flagStart)
            {
                timer_start = timer_start + 1;
            }

            if (timer_start > 50) //Button latency 0.5s
            {
                start = true;
            }

            return;
        }


        positions = haptic.stylusPositionWorld;
        rotations = haptic.stylusRotationWorld;

        // proportional control
        deltaPos = positions - PosRef;
        //Debug.Log(deltaPos);

        if (CtlType == 1)
        {
            // Proportional control
            if (deltaPos.z > res || deltaPos.z < -res)
            {

                if (deltaPos.z > 55.0f)
                    deltaPos.z = 55.0f;

                else if (deltaPos.z < - 55.0f)
                {
                    deltaPos.z = - 55.0f;
                }

                Vector3 add = transform.up * deltaPos.z * 0.02f * 0.60f; //Time.deltatime = 0.02f
                ref_position = transform.position - add;
                //Debug.Log("add" + add);
                //Debug.Log("deltaPos.z" + deltaPos.z);
                //Debug.Log("Time.deltaTime" + Time.deltaTime);

                if (deltaPos.z < res)
                {
                    ControlBackward();
                }
            }

            if (haptic.Buttons[1] == 1 && flagButton_1 == false)
            {
                circle.TurnOnCircle();
                flagButton_1 = true;
            }
                
            if (haptic.Buttons[0] == 1)
            {
                circle.CaptureCircle();
            }

            if (flagButton_1)
            {
                timer_1 = timer_1+1;
                if (timer_1 > 20)
                {
                    timer_1 = 0;
                    flagButton_1 = false;
                }
            }




        }
        else
        {
            // Go on with the button
            if (haptic.Buttons[0] == 1 && haptic.Buttons[1] == 0)
            {
                ref_position = transform.position - transform.up * Time.deltaTime * factor_t_button;
            }

            if (haptic.Buttons[1] == 1 && haptic.Buttons[0] == 0)
            {
                ref_position = transform.position + transform.up * Time.deltaTime * factor_t_button;
                ControlBackward();
            }
        }

        SetPosition();
        // Turn right/left
        if (deltaPos.x > res || deltaPos.x < -res)
        {
            //float angle_0 = angle_x;
            //angle_x = angle_x + deltaPos.x * factor_r * Time.deltaTime;

            //if (!(angle_x > -135.0f && angle_x < 135.0f))
            //{
            //    deltaPos.x = 0.0f;
            //    angle_x = angle_0;
            //}   

            Vector3 d = new Vector3(0, 0, deltaPos.x) * factor_r * Time.deltaTime;
            transform.Rotate(d, Space.Self);
        }

        // Turn up/down
        if (deltaPos.y > res || deltaPos.y < -res)
        {
            float angle_0 = angle_y;
            angle_y = angle_y - deltaPos.y * factor_r * Time.deltaTime;
            //if (!(angle_y > -135.0f && angle_y < 135.0f))
            //{
            //    deltaPos.y = 0.0f;
            //    angle_y = angle_0;
            //}

            Vector3 d = new Vector3(-deltaPos.y, 0, 0) * factor_r * Time.deltaTime;
            transform.Rotate(d, Space.Self);
        }

        //Torque right/ left

        Vector3 EulerRotations = rotations.eulerAngles;

        if (EulerRotations.z < 287.0f && EulerRotations.z > 213.0f)
        {
            float rot = 55.0f;
            float angle_0 = angle_z;
            angle_z = angle_z - rot * factor_r * Time.deltaTime;
            
            //if (!(angle_z > -135.0f && angle_z < 135.0f))
            //{
            //    rot = 0.0f;
            //    angle_z = angle_0;
            //}

            Vector3 d = new Vector3(0, -rot, 0) * factor_r * Time.deltaTime;
            transform.Rotate(d, Space.Self);

        }

        if (EulerRotations.z > 100.0f && EulerRotations.z < 147.0f)
        {
            float rot = 100.0f;
            float angle_0 = angle_z;
            //angle_z = angle_z + rot * factor_r * Time.deltaTime;

            //if (!(angle_z > -135.0f && angle_z < 135.0f))
            //{
            //    rot = 0.0f;
            //    angle_z = angle_0;
            //}

            Vector3 d = new Vector3(0, rot, 0) * factor_r * Time.deltaTime;
            transform.Rotate(d, Space.Self);

        }

    }

    void ControlBackward()
    {
        if (ccReached.CC_reached)
        {
            Quaternion pos = transform.rotation;
            Quaternion rot = Quaternion.identity;
            //ref_quaternion = pos;
            centerline.CorrectLine(15.0f, pos, ref ref_position, ref rot);
            transform.rotation = rot * pos;
            centerline.EndOfGame();
        }

    }

    void SetPosition()
    {
        if (getMesh.enable_movement)
        {
            transform.position = ref_position;
        }
        else
        {
            Vector3 ref_vector = ref_position - transform.position;
            //Debug.Log("Mesh_def" + getMesh.movement_vector);
            float product = (Vector3.Dot(ref_vector, getMesh.movement_vector));


            if (product < 0)
            {
                transform.position = ref_position;
            }

        }
    }
}

