using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mimesis_INRIA.EVEREST;

public class EndoHandle_controller2 : MonoBehaviour
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
    public float torque = 0.0f;

    bool start = false;


    public Quaternion StartRot;
    public Vector3 StartPos;
    public Vector3 direction;

    GameObject body;
    Vector3[] positions = new Vector3[100];
    Quaternion[] orientations = new Quaternion[100];

    int i = 0;
    bool move_body = false;

    public Matrix4x4 T;
    public Matrix4x4 R_x;
    public Matrix4x4 R_z;
    public Matrix4x4 R;
    public Matrix4x4 R_t;

    public Quaternion R_q;
    public Quaternion R_q_t;

    public Matrix4x4 TR;
    public Vector3 head;
    public Vector3 new_pos;

    public float rot1_scale;
    public float rot2_scale;

    public Matrix4x4 W;
    public Matrix4x4 Wn;
    public Matrix4x4 Mn;

    public Vector3 translation = new Vector3(0.0f, 0.0f, 0.0f);
    public Quaternion new_rotation;
    public Vector3 new_forward_2;
    public Vector3 new_up_2;

    GameObject reference;



    void Start()
    {
        mouseHandle = GameObject.Find("EndoHandle").GetComponent<MouseInputManager>();
        handle = GameObject.FindObjectOfType<EndoHandle>();
        reference = GameObject.Find("ref2");



        rot1_0 = handle.rotangle1;
        rot2_0 = handle.rotangle2 - 55.0f;


    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        if (!start)
        {
        Wn = transform.localToWorldMatrix;
        Vector3 scale = new Vector3(1/8.0f, 1/8.0f, 1/8.0f);
        Matrix4x4 scaleMatrix = Matrix4x4.Scale(scale);
        Wn = Wn * scaleMatrix;
        Debug.Log(Wn);

        rot1 = handle.rotangle1;
        rot2 = handle.rotangle2;
        rot2 = handle.rotangle2 - 55.0f;

        Quaternion rotation_start_1 = Quaternion.Euler(rot1, 0, 0);
        Matrix4x4 R_s1 = Matrix4x4.Rotate(rotation_start_1);

        Quaternion rotation_start_2 = Quaternion.Euler(0, 0, rot2);
        Matrix4x4 R_s2 = Matrix4x4.Rotate(rotation_start_2);

        R = R_s1 * R_s2;
        R_t = R;

        }
        start = true;

        // Get the deflection angles
        float rotation_scale = 0.01f;
        rot1 = handle.rotangle1;
        rot2 = handle.rotangle2;
        rot2 = handle.rotangle2 - 55.0f;

        rot1_scale = handle.rotangle1 * rotation_scale;
        rot2_scale = (handle.rotangle2 - 55.0f) * rotation_scale;



        deltaRot1 = (rot1 - rot1_0);
        deltaRot2 = (rot2 - rot2_0);

        // Get the insertion and torque
        insertion = mouseHandle.mouse_dy;
        torque = mouseHandle.mouse_dx;

        //Vector3 up_0 = -transform.up;


        //    if (deltaRot1 > 1.0f || deltaRot1 < -1.0f)
        //{
        //    //Quaternion q_1 = Quaternion.AngleAxis(rot1 / 1.1f, transform.forward);
        //    //transform.rotation = StartPos * q_1;

        //    Vector3 d = new Vector3(0, 0, deltaRot1) * factor_r;
        //    transform.Rotate(d, Space.Self);
        //    //rot1_0 = rot1_norm;

        //}


        //if (deltaRot2 > 1.0f || deltaRot2 < -1.0f)
        //{
        //    //Quaternion q_2 = Quaternion.AngleAxis(rot2 / 1.1f, transform.right);
        //    //transform.rotation = StartPos * q_2;

        //    Vector3 e = new Vector3(deltaRot2, 0, 0) * factor_r;
        //    transform.Rotate(e, Space.Self);


        //}



        //Translation

        if (insertion > 1.0f || insertion < -1.0f)
        {
           translation = new Vector3(0.0f, -insertion * 0.05f, 0.0f);
        }
        else translation = new Vector3(0.0f, 0.0f, 0.0f);

        T = Matrix4x4.Translate(translation);

        //Debug.Log(matrix);

        //Rotation matrix
        Quaternion rotation_1 = Quaternion.Euler(deltaRot1, 0, 0);
        Matrix4x4 R_1 = Matrix4x4.Rotate(rotation_1);

        Quaternion rotation_2 = Quaternion.Euler(0, 0, deltaRot2);
        Matrix4x4 R_2 = Matrix4x4.Rotate(rotation_2);

        R = R * R_1 * R_2;
        R_q = R.rotation;
        Vector3 R_v = R_q.eulerAngles;

        //Debug.Log("R " + R);
        //Debug.Log("R_1 " + R_1);

        //Quaternion rotation_t_1 = Quaternion.Euler(deltaRot1, 0.0f, 0.0f);
        //Matrix4x4 R_t_1 = Matrix4x4.Rotate(rotation_t_1);

        //Quaternion rotation_t_2 = Quaternion.Euler(0.0f, 0.0f, deltaRot2);
        //Matrix4x4 R_t_2 = Matrix4x4.Rotate(rotation_t_2);

        //R_t = R_t * R_t_1 * R_t_2;
        //R_q_t = Matrix4x4.rotation(R_t);
        //Vector3 R_v_t = Quaternion.eulerAngles(R_q_t);
        
        if (R_v.x > 180)
        {
            R_v.x = R_v.x - 360;
        }

        if (R_v.y > 180)
        {
            R_v.y = R_v.y - 360;
        }

        if (R_v.z > 180)
        {
            R_v.z = R_v.z - 360;
        }

        Debug.Log("R_V " + R_v);

        R_v = R_v / 100.0f;
 
        R_t = Matrix4x4.Rotate(Quaternion.Euler(R_v.x, R_v.y, R_v.z));


        //Vector3 d = new Vector3(rot1, 0, rot2);
        //Quaternion A = transform.rotation;
        //reference.transform.rotation = transform.rotation;
        //reference.transform.position = transform.position;
        //reference.transform.Rotate(d, Space.Self);
        //Quaternion B = reference.transform.rotation;
        //Quaternion C = Quaternion.Inverse(A) * B;
        //R = Matrix4x4.Rotate(C);


        if (insertion > 1.0f || insertion < -1.0f)
        {
            Wn = Wn * T * R_t;
            Mn = Wn* R;

            //Vector3 e = new Vector3(Rot1/1000, 0, Rot2/1000);
            //transform.Rotate(e, Space.Self);

        }
        else
        {
            Wn = Wn;
            Mn = Wn * R;
            //transform.Rotate(d, Space.Self);

            //Vector3 e = new Vector3(deltaRot1, 0, deltaRot2);
            //transform.Rotate(e, Space.Self);
        }




        Vector3 new_forward = new Vector3(0.0f, 0.0f, 1.0f);
        new_forward_2 = Mn.MultiplyVector(new_forward);
        Vector3 new_up = new Vector3(0.0f, 1.0f, 0.0f);
        new_up_2 = Mn.MultiplyVector(new_up);

        new_rotation = Quaternion.LookRotation(new_forward_2, new_up_2);
        transform.rotation = new_rotation;

        Vector3 centro = new Vector3(0.0f, 0.0f, 0.0f);
        new_pos = (Mn*T).MultiplyPoint(centro);
        transform.position = new_pos;


        rot1_0 = rot1;
        rot2_0 = rot2;


    }
}
