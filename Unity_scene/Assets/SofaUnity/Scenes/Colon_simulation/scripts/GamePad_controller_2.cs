using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePad_controller_2 : MonoBehaviour

{
    Joystick_controller controls;
    public float factor_t = 25f;
    public float factor_r = 55;
    float forward;
    float backward;
    // float deflect_r;
    // float deflect_l;
    // float deflect_u;
    // float deflect_d;
    float torque_r;
    float torque_l;
    Vector2 deflect;
    public bool m_isactive = true;

    public GameObject ActiveCircle;
    private CircleGraphic circle;
    private ActivateCircle circle_script;

    Vector3 ref_position;
    Quaternion ref_quaternion;
    public bool enable_movement = true;
    protected GetChangeMesh_2 getMesh;

    protected Lines centerline;
    public CoinPicker ccReached;

    public float angle_x = 0.0f;
    public float angle_y = 0.0f;
    public float angle_z = 0.0f;


    void Start()
    {
        ActiveCircle = GameObject.Find("Circle canvas");
        //circle = ActiveCircle.GetComponent<CircleGraphic>();
        circle_script = ActiveCircle.GetComponent<ActivateCircle>();

        ref_position = transform.position;

        getMesh = GameObject.Find("OglModel  -  ColonVisualModel").GetComponent<GetChangeMesh_2>();

        //Backward control
        centerline = GameObject.Find("Line").GetComponent<Lines>();
        ccReached = this.GetComponent<CoinPicker>();
    }
    void Awake()
    {
        controls = new Joystick_controller();

        controls.Movements.Go_forward.performed += ctx => forward = ctx.ReadValue<float>();
        controls.Movements.Go_forward.canceled += ctx => forward = 0.0f;

        controls.Movements.Go_backward.performed += ctx => backward = ctx.ReadValue<float>();
        controls.Movements.Go_backward.canceled += ctx => backward = 0.0f;

        controls.Movements.Torque_right.performed += ctx => torque_r = ctx.ReadValue<float>();
        controls.Movements.Torque_right.canceled += ctx => torque_r = 0.0f;

        controls.Movements.Torque_left.performed += ctx => torque_l = ctx.ReadValue<float>();
        controls.Movements.Torque_left.canceled += ctx => torque_l = 0.0f;

        controls.Movements.Deflect.performed += ctx => deflect = ctx.ReadValue<Vector2>();
        controls.Movements.Deflect.canceled += ctx => deflect = Vector2.zero;

        controls.Movements.Activate.performed += ctx => TurnOnCircle();
        controls.Movements.Capture.performed += ctx => SaveCircle();

        //controls.Movements.Speed_up.performed += ctx => SpeedUp();
        //controls.Movements.Speed_down.performed += ctx => SpeedDown();


    }

    void FixedUpdate()
    {

        if (!m_isactive)
            return;
        ref_position = transform.position - transform.up * forward * 0.02f * factor_t + transform.up * backward * 0.02f * factor_t;
        //Debug.Log("Add" + transform.up * forward * 0.02f * factor_t);

        if (backward > 0.1f )
        {
            ControlBackward();
        }

        // ref_position = transform.position + transform.up * backward * Time.deltaTime * factor_t;
        float angle_0_z = angle_z;
        //angle_z = angle_z + (torque_r - torque_l) * factor_r * Time.deltaTime;

        //if (!(angle_z > -135.0f && angle_z < 135.0f))
        //{
        //    torque_r = 0.0f;
        //    torque_l = 0.0f;
        //    angle_z = angle_0_z;
        //}

        Quaternion torq_r = Quaternion.Euler(0.0f, torque_r, 0.0f);
        transform.rotation = transform.rotation * torq_r;

        Quaternion torq_l = Quaternion.Euler(0.0f, -torque_l, 0.0f);
        transform.rotation = transform.rotation * torq_l;

        //float angle_0_x = angle_x;
        //float angle_0_y = angle_y;

        //angle_x = angle_x + deflect.x * factor_r * Time.deltaTime;
        //angle_y = angle_y + deflect.y * factor_r * Time.deltaTime;
        //if (!(angle_x > -135.0f && angle_x < 135.0f))
        //{
        //    deflect.x = 0.0f;
        //    angle_x = angle_0_x;
        //}
            
        //if (!(angle_y > -135.0f && angle_y < 135.0f))
        //{
        //    deflect.y = 0.0f;
        //    angle_y = angle_0_y;
        //}

        Vector3 d = new Vector3(-deflect.y, 0.0f, deflect.x) * factor_r * Time.deltaTime;

        transform.Rotate(d, Space.Self);

        SetPosition();
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


    void OnEnable()
    {
        controls.Movements.Enable();
    }

    void OnDisable()
    {
        controls.Movements.Disable();
    }

    void TurnOnCircle()
    {
        circle_script.TurnOnCircle();
    }

    void SaveCircle()
    {
        circle_script.CaptureCircle();
 
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

    void SpeedUp()
    {
        if (factor_t < 30)
        {
            factor_t = factor_t + 5.0f;
            factor_r = factor_r + 15.0f;

        }

    }

    void SpeedDown()
    {
        if (factor_t > 5)
        {
            factor_t = factor_t - 5.0f;
            factor_r = factor_r - 15.0f;

        }

    }
}