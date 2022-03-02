using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mimesis_INRIA.EVEREST;

public class EndoHandle_controller3 : MonoBehaviour
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
    protected GetChangeMesh_2 getMesh;


    void Start()
    {
       // mouseHandle = GameObject.Find("EndoHandle").GetComponent<MouseInputManager>();
        handle = GameObject.FindObjectOfType<EndoHandle>();


        rot1_0 = handle.rotangle1;
        rot2_0 = handle.rotangle2 - 55.0f;

        Base = GameObject.Find("Base");
        Base.transform.rotation = transform.rotation;
        Base.transform.position = transform.position + transform.up * (Lf + Lt / 2.0f);

        getMesh = GameObject.Find("OglModel  -  ColonVisualModel").GetComponent<GetChangeMesh_2>();

        //getMesh = GameObject.Find("OglModel  -  ColonVisualModel").GetComponent<GetChangeMesh_2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!start)
        {
      
        }
        start = true;

        // Get the deflection angles
        float rotation_scale = 0.01f;
        rot1 = handle.rotangle1;
        rot2 = handle.rotangle2;

        rot1 = Mathf.Ceil(handle.rotangle1/119.0f*75.0f);
        rot2 = Mathf.Ceil((handle.rotangle2 - 55.0f)/154.0f * 75.0f); //-55

        //rot2 = handle.rotangle2 - 76.0f;

        deltaRot1 = (rot1 - rot1_0);
        deltaRot2 = (rot2 - rot2_0);

        // Get the insertion and torque
        //insertion = mouseHandle.mouse_dy;

        if (Input.GetKey(KeyCode.K))
        {
            insertion = 1.5f * t_factor;
            Debug.Log("inserted");
        }

        else if (Input.GetKey(KeyCode.L))
        {
            insertion = - 2.5f * t_factor;
            Debug.Log("back");
        }

        else insertion = 0.0f;

        //torque = mouseHandle.mouse_dx;


        rot1_rad = rot1 / 180.0f * Mathf.PI;
        rot2_rad = rot2 / 180.0f * Mathf.PI;
        beta = 2.0f * (Rp / D) * Mathf.Sqrt(rot1_rad * rot1_rad + rot2_rad * rot2_rad);
        
        if (Mathf.Abs(rot1)< 0.1f && Mathf.Abs(rot2) < 0.1f)
        {
            alfa = 0.0f;
        }
        else alfa = Mathf.Atan2(-rot1_rad, -rot2_rad);

        beta_ins = beta * insertion * 0.01f / Lf;
        

        //Matrices
        Vector4 raw1 = new Vector4(Mathf.Sin(alfa) * Mathf.Sin(alfa) + Mathf.Cos(beta) * Mathf.Cos(alfa) * Mathf.Cos(alfa), -Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta)), Mathf.Cos(alfa) * Mathf.Sin(beta), Lt * Mathf.Cos(alfa) * Mathf.Sin(beta) + (Lf / beta) * (1 - Mathf.Cos(beta)) * Mathf.Cos(alfa));
        Vector4 raw2 = new Vector4(-Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta)), Mathf.Cos(alfa) * Mathf.Cos(alfa) + Mathf.Cos(beta) * Mathf.Sin(alfa) * Mathf.Sin(alfa), Mathf.Sin(alfa) * Mathf.Sin(beta), Lt * Mathf.Sin(alfa) * Mathf.Sin(beta) + (Lf / beta) * (1 - Mathf.Cos(beta)) * Mathf.Sin(alfa));
        Vector4 raw3 = new Vector4(-Mathf.Cos(alfa) * Mathf.Sin(beta), -Mathf.Sin(alfa) * Mathf.Sin(beta), Mathf.Cos(beta), Lt * Mathf.Cos(beta) + (Lf / beta) * Mathf.Sin(beta));
        Vector4 raw4 = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        Debug.Log("Cos " + Mathf.Cos(beta) + "Sin " + Mathf.Sin(beta));
        Debug.Log("Z transaltion" + (Lt * Mathf.Cos(beta) + (Lf / beta) * Mathf.Sin(beta)));

        R.SetRow(0, raw1);
        R.SetRow(1, raw2);
        R.SetRow(2, raw3);
        R.SetRow(3, raw4);

        Transformation.SetRow(0, new Vector4(1.0f, 0.0f, 0.0f, 0.0f));
        Transformation.SetRow(1, new Vector4(0.0f, 0.0f, 1.0f, 0.0f));
        Transformation.SetRow(2, new Vector4(0.0f, -1.0f, 0.0f, 0.0f));
        Transformation.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

        // T = rotation + translation + transformation
        T = Transformation.inverse* R * Transformation;

        Mb = Matrix4x4.Rotate(Base.transform.rotation);
        Vector4 pos_base = new Vector4(Base.transform.position.x, Base.transform.position.y, Base.transform.position.z, 1.0f);
        Mb.SetColumn(3, pos_base);

        W2C = Mb * T;

        Quaternion qt = Quaternion.LookRotation(W2C.GetColumn(2), W2C.GetColumn(1));
        transform.rotation = qt;
        Vector4 pos4 = W2C.GetColumn(3);
        Vector3 pos = pos4;
        transform.position = pos;


        //insertion
        //Matrices
        //raw1 = new Vector4(Mathf.Sin(alfa) * Mathf.Sin(alfa) + Mathf.Cos(beta_ins) * Mathf.Cos(alfa) * Mathf.Cos(alfa), -Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta_ins)), Mathf.Cos(alfa) * Mathf.Sin(beta_ins), Mathf.Sin(beta_ins) + (Lf / beta) * (1 - Mathf.Cos(beta_ins)) * Mathf.Cos(alfa));
        //raw2 = new Vector4(-Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta_ins)), Mathf.Cos(alfa) * Mathf.Cos(alfa) + Mathf.Cos(beta_ins) * Mathf.Sin(alfa) * Mathf.Sin(alfa), Mathf.Sin(alfa) * Mathf.Sin(beta_ins), Mathf.Sin(beta_ins) + (Lf / beta) * (1 - Mathf.Cos(beta_ins)) * Mathf.Sin(alfa));
        raw1 = new Vector4(Mathf.Sin(alfa) * Mathf.Sin(alfa) + Mathf.Cos(beta_ins) * Mathf.Cos(alfa) * Mathf.Cos(alfa), -Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta_ins)), Mathf.Cos(alfa) * Mathf.Sin(beta_ins), (Lf / beta) * (1 - Mathf.Cos(beta_ins)) * Mathf.Cos(alfa));
        raw2 = new Vector4(-Mathf.Sin(alfa) * Mathf.Cos(alfa) * (1 - Mathf.Cos(beta_ins)), Mathf.Cos(alfa) * Mathf.Cos(alfa) + Mathf.Cos(beta_ins) * Mathf.Sin(alfa) * Mathf.Sin(alfa), Mathf.Sin(alfa) * Mathf.Sin(beta_ins), (Lf / beta) * (1 - Mathf.Cos(beta_ins)) * Mathf.Sin(alfa));
        raw3 = new Vector4(-Mathf.Cos(alfa) * Mathf.Sin(beta_ins), -Mathf.Sin(alfa) * Mathf.Sin(beta_ins), Mathf.Cos(beta_ins), (Lf / beta) * Mathf.Sin(beta_ins));
        raw4 = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        R_ins.SetRow(0, raw1);
        R_ins.SetRow(1, raw2);
        R_ins.SetRow(2, raw3);
        R_ins.SetRow(3, raw4);

        Matrix4x4 T_ins = Transformation.inverse * R_ins * Transformation;
        Matrix4x4 W2B= Mb * T_ins;

        Quaternion qt_b = Quaternion.LookRotation(W2B.GetColumn(2), W2B.GetColumn(1));
        Base.transform.rotation = qt_b;
        Vector4 pos_b_4 = W2B.GetColumn(3);
        Vector3 pos_b = pos_b_4;
        Base.transform.position = pos_b;


        ////Translation

        //if (insertion > 1.0f || insertion < -1.0f)
        //{
        //    translation = new Vector3(0.0f, -insertion * 0.05f, 0.0f);
        //}
        //else translation = new Vector3(0.0f, 0.0f, 0.0f);

        //T = Matrix4x4.Translate(translation);

        ////Debug.Log(matrix);

        ////Rotation matrix
        //Quaternion rotation_1 = Quaternion.Euler(deltaRot1, 0, 0);
        //Matrix4x4 R_1 = Matrix4x4.Rotate(rotation_1);

        //Quaternion rotation_2 = Quaternion.Euler(0, 0, deltaRot2);
        //Matrix4x4 R_2 = Matrix4x4.Rotate(rotation_2);

        //R = R * R_1 * R_2;
        //R_q = R.rotation;
        //Vector3 R_v = R_q.eulerAngles;

        ////Debug.Log("R " + R);
        ////Debug.Log("R_1 " + R_1);

        ////Quaternion rotation_t_1 = Quaternion.Euler(deltaRot1, 0.0f, 0.0f);
        ////Matrix4x4 R_t_1 = Matrix4x4.Rotate(rotation_t_1);

        ////Quaternion rotation_t_2 = Quaternion.Euler(0.0f, 0.0f, deltaRot2);
        ////Matrix4x4 R_t_2 = Matrix4x4.Rotate(rotation_t_2);

        ////R_t = R_t * R_t_1 * R_t_2;
        ////R_q_t = Matrix4x4.rotation(R_t);
        ////Vector3 R_v_t = Quaternion.eulerAngles(R_q_t);

        //if (R_v.x > 180)
        //{
        //    R_v.x = R_v.x - 360;
        //}

        //if (R_v.y > 180)
        //{
        //    R_v.y = R_v.y - 360;
        //}

        //if (R_v.z > 180)
        //{
        //    R_v.z = R_v.z - 360;
        //}

        //Debug.Log("R_V " + R_v);

        //R_v = R_v / 100.0f;

        //R_t = Matrix4x4.Rotate(Quaternion.Euler(R_v.x, R_v.y, R_v.z));


        ////Vector3 d = new Vector3(rot1, 0, rot2);
        ////Quaternion A = transform.rotation;
        ////reference.transform.rotation = transform.rotation;
        ////reference.transform.position = transform.position;
        ////reference.transform.Rotate(d, Space.Self);
        ////Quaternion B = reference.transform.rotation;
        ////Quaternion C = Quaternion.Inverse(A) * B;
        ////R = Matrix4x4.Rotate(C);


        //if (insertion > 1.0f || insertion < -1.0f)
        //{
        //    Wn = Wn * T * R_t;
        //    Mn = Wn * R;

        //    //Vector3 e = new Vector3(Rot1/1000, 0, Rot2/1000);
        //    //transform.Rotate(e, Space.Self);

        //}
        //else
        //{
        //    Wn = Wn;
        //    Mn = Wn * R;
        //    //transform.Rotate(d, Space.Self);

        //    //Vector3 e = new Vector3(deltaRot1, 0, deltaRot2);
        //    //transform.Rotate(e, Space.Self);
        //}




        //Vector3 new_forward = new Vector3(0.0f, 0.0f, 1.0f);
        //new_forward_2 = Mn.MultiplyVector(new_forward);
        //Vector3 new_up = new Vector3(0.0f, 1.0f, 0.0f);
        //new_up_2 = Mn.MultiplyVector(new_up);

        //new_rotation = Quaternion.LookRotation(new_forward_2, new_up_2);
        //transform.rotation = new_rotation;

        //Vector3 centro = new Vector3(0.0f, 0.0f, 0.0f);
        //new_pos = (Mn * T).MultiplyPoint(centro);
        //transform.position = new_pos;


        rot1_0 = rot1;
        rot2_0 = rot2;


    }
}

