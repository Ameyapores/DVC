using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mimesis_INRIA.EVEREST;

public class EndoHandle_controller4 : MonoBehaviour
{
    EndoHandle handle;
    MouseInputManager mouseHandle;
    public float factor = 0.1f;
    private float maxRot1 = 118.0f;
    private float minRot1 = -118.0f;
    private float maxRot2 = 209.0f; //-160g
    private float minRot2 = -99.0f; //+160g
    public float rot1 = 0.0f;
    public float rot2 = 0.0f;
    public float rot1_0 = 0.0f;
    public float rot2_0 = 0.0f;
    public float deltaRot1 = 0.0f;
    public float deltaRot2 = 0.0f;
    public float factor_t = 1.0f;
    public float factor_r = 1.0f;
    public float rot2_norm = 0.0f;
    public float rot1_norm = 0.0f;

    public float insertion = 0.0f;

    public bool start = false;
    public Matrix4x4 R;
    public Matrix4x4 T;
    public Matrix4x4 Transformation;

    public float alfa;
    public float beta;
    public float Lt = 10.0f;
    public float Lf = 30.0f;
    public float Rp = 10.0f; //1cm
    public float D = 15.0f; //1.5cm
    public float dl_1 = 0.0f;
    public float dl_2 = 0.0f;

    public GameObject Base;
    public float rot1_rad;
    public float rot2_rad;
    Matrix4x4 Mb;
    Matrix4x4 W2C;
    public Matrix4x4 R_ins;
    public float beta_ins;

    public float t_factor = 20.0f;
    public float r_factor = 1.0f;
    protected GetChangeMesh_2 getMesh;

    Vector3 ref_position;
    Quaternion ref_quaternion;

    public Vector3 contact_position;
    GameObject contact_capsule;

    public int counter = 0;

    protected Lines centerline;
    public CoinPicker ccReached;
    public float torque = 0.0f;

    float x_T_R;
    float y_T_R;
    float z_T_R;

    float x_T_Rins;
    float y_T_Rins;
    float z_T_Rins;

    private ActivateCircle circle_script;
    public GameObject ActiveCircle;
    private CircleGraphic circle;
    bool flagButton = false;
    int timer_1 = 0; 


    void Start()
    {
        mouseHandle = GameObject.Find("EndoHandle").GetComponent<MouseInputManager>();
        handle = GameObject.FindObjectOfType<EndoHandle>();

        rot1_0 = handle.rotangle1;
        rot2_0 = handle.rotangle2 - 55.0f;

        Base = GameObject.Find("Base");
        Base.transform.rotation = transform.rotation;
        Base.transform.position = transform.position + transform.up * (Lf + Lt / 2.0f);

        getMesh = GameObject.Find("OglModel  -  ColonVisualModel").GetComponent<GetChangeMesh_2>();

        contact_capsule = GameObject.Find("Capsule_contact");

        contact_capsule.transform.rotation = transform.rotation;
        contact_capsule.transform.position = transform.position;

        centerline = GameObject.Find("Line").GetComponent<Lines>();
        ccReached = this.GetComponent<CoinPicker>();

        ActiveCircle = GameObject.Find("Circle canvas");
        circle_script = ActiveCircle.GetComponent<ActivateCircle>();

    }

    // Update is called once per frame
    void FixedUpdate()
    { 

        // Get the deflection angles
        float rotation_scale = 0.01f;
        rot1 = handle.rotangle1;
        rot2 = handle.rotangle2;

        rot1 = Mathf.Ceil(handle.rotangle1 / 119.0f * 75.0f);
        rot2 = Mathf.Ceil((handle.rotangle2 - 55.0f) / 154.0f * 75.0f); //-55

        //rot2 = handle.rotangle2 - 76.0f;

        deltaRot1 = (rot1 - rot1_0);
        deltaRot2 = (rot2 - rot2_0);

        // Get the insertion and torque
        insertion = mouseHandle.mouse_dy;
        //Debug.Log("Insertion " + insertion);

        if (Input.GetKey(KeyCode.Space))
        {
            insertion = 0.0f;
            //Debug.Log("inserted");
        }

        //if (Input.GetKey(KeyCode.K))
        //{
        //    insertion = 2.5f * t_factor;
        //    Debug.Log("inserted");
        //}

        //else if (Input.GetKey(KeyCode.L))
        //{
        //    insertion = -2.5f * t_factor;
        //    Debug.Log("back");
        //}

        //else insertion = 0.0f;

        //TORQUE
        torque = mouseHandle.mouse_dx;
        //if (Input.GetKey(KeyCode.N))
        //{
        //    torque = 2.5f * r_factor;
        //    Debug.Log("torque right");
        //}

        //else if (Input.GetKey(KeyCode.B))
        //{
        //    torque = -2.5f * r_factor;
        //    Debug.Log("torque left");
        //}

        //else torque = 0.0f;

        rot1_rad = rot1 / 180.0f * Mathf.PI;
        rot2_rad = rot2 / 180.0f * Mathf.PI;
        beta = 2.0f * (Rp / D) * Mathf.Sqrt(rot1_rad * rot1_rad + rot2_rad * rot2_rad);

        if (Mathf.Abs(rot1) < 0.1f && Mathf.Abs(rot2) < 0.1f)
        {
            alfa = 0.0f;
        }
        else alfa = Mathf.Atan2(-rot1_rad, -rot2_rad);

        if (Mathf.Abs(beta) > 0.001f)
        { 
            x_T_R =  Lt * Mathf.Cos(alfa) * Mathf.Sin(beta) + (Lf / beta) * (1 - Mathf.Cos(beta)) * Mathf.Cos(alfa);
            y_T_R = Lt * Mathf.Sin(alfa) * Mathf.Sin(beta) + (Lf / beta) * (1 - Mathf.Cos(beta)) * Mathf.Sin(alfa);
            z_T_R = Lt * Mathf.Cos(beta) + (Lf / beta) * Mathf.Sin(beta);
        }
        else
        {
            x_T_R = 0.0f;
            y_T_R = 0.0f;
            z_T_R = Lt + Lf;

        }

        //Matrices
        Vector4 raw1 = new Vector4(Mathf.Sin(alfa) * Mathf.Sin(alfa) + Mathf.Cos(beta) * Mathf.Cos(alfa) * Mathf.Cos(alfa), -Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta)), Mathf.Cos(alfa) * Mathf.Sin(beta), x_T_R);
        Vector4 raw2 = new Vector4(-Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta)), Mathf.Cos(alfa) * Mathf.Cos(alfa) + Mathf.Cos(beta) * Mathf.Sin(alfa) * Mathf.Sin(alfa), Mathf.Sin(alfa) * Mathf.Sin(beta), y_T_R);
        Vector4 raw3 = new Vector4(-Mathf.Cos(alfa) * Mathf.Sin(beta), -Mathf.Sin(alfa) * Mathf.Sin(beta), Mathf.Cos(beta), z_T_R);
        Vector4 raw4 = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

      //  Debug.Log("Cos " + Mathf.Cos(beta) + "Sin " + Mathf.Sin(beta));
      //  Debug.Log("Z transaltion" + (Lt * Mathf.Cos(beta) + (Lf / beta) * Mathf.Sin(beta)));

        R.SetRow(0, raw1);
        R.SetRow(1, raw2);
        R.SetRow(2, raw3);
        R.SetRow(3, raw4);
        Debug.Log("R "+ R);

        Transformation.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
        Transformation.SetRow(1, new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
        Transformation.SetRow(2, new Vector4(0.0f, -1.0f, 0.0f, 0.0f));
        Transformation.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

        // T = rotation + translation + transformation
        T = Transformation.inverse * R * Transformation;

        Mb = Matrix4x4.Rotate(Base.transform.rotation);
        Vector4 pos_base = new Vector4(Base.transform.position.x, Base.transform.position.y, Base.transform.position.z, 1.0f);
        Mb.SetColumn(3, pos_base);

        W2C = Mb * T;

        Quaternion qt = Quaternion.LookRotation(W2C.GetColumn(2), W2C.GetColumn(1));

        //transform.rotation = qt;
        ref_quaternion = qt;

        Vector4 pos4 = W2C.GetColumn(3);
        Vector3 pos = pos4;
        //transform.position = pos;

        ref_position = pos;

        if (counter > 1 ) SetPosition();

        //Insertion
        if ((insertion < 0.0f && centerline.centerline_index < 31)|| insertion > 0.0f) //(insertion > 0.0f || !ccReached.CC_reached)
        {

            beta_ins = beta * insertion * 0.01f / Lf;

            if (Mathf.Abs(beta) > 0.001f)
            {
                x_T_Rins =  (Lf / beta) * (1 - Mathf.Cos(beta_ins)) * Mathf.Cos(alfa);
                y_T_Rins =  (Lf / beta) * (1 - Mathf.Cos(beta_ins)) * Mathf.Sin(alfa);
                z_T_Rins = (Lf / beta) * Mathf.Sin(beta_ins);
            }
            else
            {
                x_T_Rins = 0.0f;
                y_T_Rins = 0.0f;
                z_T_Rins = insertion * 0.01f;

            }

            //insertion
            raw1 = new Vector4(Mathf.Sin(alfa) * Mathf.Sin(alfa) + Mathf.Cos(beta_ins) * Mathf.Cos(alfa) * Mathf.Cos(alfa), -Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta_ins)), Mathf.Cos(alfa) * Mathf.Sin(beta_ins), x_T_Rins);
            raw2 = new Vector4(-Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta_ins)), Mathf.Cos(alfa) * Mathf.Cos(alfa) + Mathf.Cos(beta_ins) * Mathf.Sin(alfa) * Mathf.Sin(alfa), Mathf.Sin(alfa) * Mathf.Sin(beta_ins), y_T_Rins);
            raw3 = new Vector4(-Mathf.Cos(alfa) * Mathf.Sin(beta_ins), -Mathf.Sin(alfa) * Mathf.Sin(beta_ins), Mathf.Cos(beta_ins), z_T_Rins);
            raw4 = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

            R_ins.SetRow(0, raw1);
            R_ins.SetRow(1, raw2);
            R_ins.SetRow(2, raw3);
            R_ins.SetRow(3, raw4);

            Matrix4x4 T_ins = Transformation.inverse * R_ins * Transformation;
            Matrix4x4 W2B = Mb * T_ins;

            Quaternion qt_b = Quaternion.LookRotation(W2B.GetColumn(2), W2B.GetColumn(1));
            Base.transform.rotation = qt_b;
            Vector4 pos_b_4 = W2B.GetColumn(3);
            Vector3 pos_b = pos_b_4;
            Base.transform.position = pos_b;

        }

        //Retraction
        if ((insertion < 0.0f && centerline.centerline_index > 30)) //if (insertion < 0.0f || ccReached.CC_reached)
        {
            Vector3 ref_position_base = Base.transform.position - transform.up * insertion * Time.deltaTime;
            Quaternion rot_base = Base.transform.rotation;
            Quaternion rot = Quaternion.identity;
            //ref_quaternion = pos;
            centerline.CorrectLine(15.0f, rot_base, ref ref_position_base, ref rot);
            Base.transform.rotation =  rot * rot_base;
            Base.transform.position = ref_position_base;
        }

        Debug.Log("index" + centerline.centerline_index);

        if (Mathf.Abs(torque)>1.0f)
        {
                Quaternion rot = Quaternion.Euler(0, -torque/20.0f, 0);
                Base.transform.rotation = Base.transform.rotation * rot;
        }
        

        rot1_0 = rot1;
        rot2_0 = rot2;


        if (counter < 2) 
        {  
            transform.position =contact_capsule.transform.position;
            transform.rotation = contact_capsule.transform.rotation;
            Debug.Log("Not started"  + counter);
            
        }

        counter = counter + 1;

        // if (!start) 
        // { 
        //     counter  = counter +1;

        //     if (counter == 5) 
        //     {
        //         start = true;
        //         counter = 0;
        //     }

        // }

        // Debug.Log("contact_capsule "+ contact_capsule.transform.position + "capsule "+transform.position);
        if (handle.Buttons[2] == true && flagButton == false)
        {
            TurnOnCircle();
            flagButton = true;
        }

        if (handle.Buttons[1] == true)
        {
            SaveCircle();
        }

        if (flagButton)
        {
            timer_1 = timer_1 + 1;
            if (timer_1 > 20)
            {
                timer_1 = 0;
                flagButton = false;
            }
        }

    }

    void SetPosition()
    {

        if (!getMesh.enable_movement)
        {
            Vector3 ref_vector = ref_position-transform.position;
            //Debug.Log("Mesh_def" + getMesh.movement_vector);
            float product = (Vector3.Dot(ref_vector, getMesh.movement_vector));
            
            if (product < 0)
            {
                contact_capsule.transform.position = ref_position;
                contact_capsule.transform.rotation = ref_quaternion;
                controlVelocity();
            }
        }
        
        else 
        {
            contact_capsule.transform.position = ref_position;
            contact_capsule.transform.rotation = ref_quaternion;
            controlVelocity();
        }
    }

    void controlVelocity()
    {
        float dist = Vector3.Distance(contact_capsule.transform.position, transform.position);
        //Debug.Log("Distance" + dist);
        float threshold = 35.0f * Time.deltaTime;

    //If the capsule goes faster than the maximum velocity allowed by the simulator
        if (dist > threshold)
        {
            //Vector3 newPos = Vector3.Normalize(contact_capsule.transform.position)*25.0f;
            Vector3 newPos = transform.position + Vector3.Normalize(contact_capsule.transform.position-transform.position)*threshold;
            Debug.DrawLine(transform.position, newPos, Color.red, 2.5f);

            transform.position = newPos;
            transform.rotation = contact_capsule.transform.rotation;
        }
        else 
        {
            transform.position = contact_capsule.transform.position;
            transform.rotation = contact_capsule.transform.rotation;

        }
    }
    // bool contact = contact_capsule.GetComponent<Collision_capsule>().collided;
    // if (!contact)
    // {
    //     if (start)
    //     {
    //         //Dopo il primo t, inizio ad assegnare alla capsula le posizione di capsule_contact
    //     transform.position = ref_position;
    //     transform.rotation = ref_quaternion;
    //     }

    //     ref_position = contact_capsule.transform.position;
    //     ref_quaternion = contact_capsule.transform.rotation;

    //     start = false;
    // }
    //if 
    // if (getMesh.deformation)
    // {
    //     Debug.Log("Deformed");
    // contact_position = transform.position;

    // Vector3 ref_vector = ref_position - transform.position;
    // Debug.Log("Mesh_def" + getMesh.movement_vector_0);
    // Debug.Log("ref_vector" + ref_vector);

    // float product = (Vector3.Dot(ref_vector, getMesh.movement_vector_0));
    // Debug.DrawLine(getMesh.movement_vector_0, transform.position + getMesh.movement_vector_0, Color.red, 2.5f);
    //Debug.Log("ref_vector " + ref_position + "transform.position " + transform.position);
    //Debug.Log("product " + product);




    // if (product < 0)
    // {
    //     transform.position = ref_position;
    //     transform.rotation = ref_quaternion;
    // }
    // }
    // else
    // {
    //     transform.position = ref_position;
    //     transform.rotation = ref_quaternion;
    // }

    //else if (getMesh.enable_movement)
    //{
    //    transform.position = ref_position;
    //    transform.rotation = ref_quaternion;
    //}
    //else
    //{
    //    Vector3 ref_vector = ref_position - transform.position;
    //    //Debug.Log("Mesh_def" + getMesh.movement_vector);
    //    float product = (Vector3.Dot(ref_vector, getMesh.movement_vector));


    //    if (product < 0)
    //    {
    //        transform.position = ref_position;
    //        transform.rotation = ref_quaternion;
    //    }

    //}

    void TurnOnCircle()
    {
        circle_script.TurnOnCircle();
    }

    void SaveCircle()
    {
        circle_script.CaptureCircle();

    }

}

