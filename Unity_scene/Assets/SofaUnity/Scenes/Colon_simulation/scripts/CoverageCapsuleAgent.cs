using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using SofaUnity;

using System.Linq;
using static System.Math;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEditor;

public class CoverageCapsuleAgent : Agent
{
    public SofaContext m_sofa;
    //public GetChangeMesh_2 m_collision;
    [SerializeField] GameObject centroid;
    [SerializeField] GameObject lines_dir;
    [SerializeField] GameObject text_render;
    // private GameObject coin;
    float reward ;
    float currVelo;
    float safeVelo = 2.3f, beta = 0.1f;
    float torque_value = 5.0f;
    float translation_speed = 10f;
    // object parameters
    private Rigidbody capsule_rbody;
    private image_processing centroid_pos;
    private Lines direction;
    
    // public GameObject capsule;
    //public GameObject arrow;
    private Vector3 capsule_init_pos;
    private Vector3 capsule_prev_pos;
    private Quaternion capsule_init_rot;
    
    private Text image_text;

    //for CT
    // private const float CAPSULE_YLIMIT_LOW = -200f, CAPSULE_YLIMIT_UP = 180.0f;
    // private const float CAPSULE_ZLIMIT_LOW = -150f, CAPSULE_ZLIMIT_UP = 130.0f;
    // private const float CAPSULE_XLIMIT_LOW = -190f, CAPSULE_XLIMIT_UP = 190f;
    
    //for C1
    // private const float CAPSULE_YLIMIT_LOW = -350f, CAPSULE_YLIMIT_UP = -75.0f;
    // private const float CAPSULE_ZLIMIT_LOW = -455.5f;

    // for C2 and C3
    // private const float CAPSULE_YLIMIT_LOW = -350f, CAPSULE_YLIMIT_UP = -120.0f;
    // for C2
    // private const float CAPSULE_ZLIMIT_LOW = -450.5f;

    //for C3
    // private const float CAPSULE_ZLIMIT_LOW = -445.5f;

    //for C1 , C2 and C3
    // private const float CAPSULE_XLIMIT_LOW = -150f, CAPSULE_XLIMIT_UP = 150f;

    // private const float CAPSULE_VELOCITY_MAX = 3.0f;
    // private const float CAPSULE_ANGULAR_VELOCITY_MAX = 0.5f;

    int force_direction_x = 0;
    int force_direction_y = 0;
    int force_direction_z = 0;
    int torque_direction_x = 0;
    int torque_direction_y = 0;
    int torque_direction_z = 0;

    int index_centerline = 0 ; 

    private CoinPicker cointouch;

    bool collision_flag ;
    bool manual_control = false;
    bool manual_flag = false;
    bool distance_flag = false;
    bool emergency_stop;
    // other parameters
    // private float prev_coverage = 0;
    //private Vector3 action_pos_vector;
    //private Vector3 action_rot_vector;
    int count = 0;
    int count2;
    bool fellOff = false;

    void Start () {
        capsule_rbody = this.GetComponent<Rigidbody>();
        centroid_pos = centroid.GetComponent<image_processing>();
        capsule_init_pos = this.transform.position;
        capsule_init_rot = this.transform.rotation;
        capsule_prev_pos = this.transform.position;
        // coin = GameObject.FindGameObjectWithTag(CC);
        cointouch = this.GetComponent<CoinPicker>();
        direction = lines_dir.GetComponent<Lines>();

        image_text = text_render.GetComponent<Text>();
        image_text.enabled = false;

    }
    
    public override void OnEpisodeBegin()
    {   
        // Reset the scene parameters
        Debug.Log("NEW EPISODE!");
        // capsule_rbody.angularVelocity = Vector3.zero;
        // capsule_rbody.velocity = Vector3.zero;
        this.transform.position = capsule_init_pos;
        this.transform.rotation = capsule_init_rot;
        capsule_prev_pos = capsule_init_pos;
        m_sofa.ResetSofa();            
        manual_control = false;
        manual_flag =  false;
        count = 0;
        count2 = 0;
        fellOff = false;
        cointouch.CC_reached=false;
        emergency_stop = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent positions
        //sensor.AddObservation(this.transform.position);
        //sensor.AddObservation(capsule_rbody.velocity.magnitude);
        //sensor.AddObservation(m_collision.getcollision() ? 0 :1);
        // sensor.AddObservation(cointouch.CC_reached ? 0 :1);
        //new ones
        // sensor.AddObservation(centroid_pos.Getdistance());
        // sensor.AddObservation(centroid_pos.Get_x_value());
        // sensor.AddObservation(centroid_pos.Get_y_value());
        // sensor.AddObservation(capsule_rbody.angularVelocity.magnitude);
        // sensor.AddObservation(this.transform.rotation);
        // sensor.AddObservation(direction.Get_i_value());
    }
    public override void OnActionReceived(float[] action)
    {

        reward = 0;
        count ++;
        int torque_y = Mathf.FloorToInt(action[0]);
        int torque_z = Mathf.FloorToInt(action[1]);
        int torque_x = Mathf.FloorToInt(action[2]);

        if (torque_y == 0) {  torque_direction_y = -1; }
        if (torque_y == 1) {  torque_direction_y = 0; }
        if (torque_y == 2) {  torque_direction_y = 1; }

        if (torque_z == 0) {  torque_direction_z = -1; }
        if (torque_z == 1) {  torque_direction_z = 0; }
        if (torque_z == 2) {  torque_direction_z = 1; }

        if (torque_x == 0) {  torque_direction_x = -1; }
        if (torque_x == 1) {  torque_direction_x = 0; }
        if (torque_x == 2) {  torque_direction_x = 1; }
        
        // Debug.Log("count"+ count);
        float distance = centroid_pos.Getdistance();
        // Debug.Log("manual_control"+ manual_control);


        if (distance == 1.0f && count >1){ manual_flag = true;}
        
        if (manual_flag ==true)
        {
            count2 ++;
            if (count2>=30){manual_control =true;}
        }    

        
        if (distance == 1.0f | manual_control ==true)
        // if (distance == 1.0f)
        {
            // capsule_rbody.AddRelativeTorque(new Vector3(torque_direction_x * torque_value, torque_direction_y * torque_value , torque_direction_z* torque_value));
            // reward -= 3f;
            // AddReward(reward);
            // capsule_rbody.velocity = transform.up* 0f;
            if (manual_control == true)
            {
                image_text.enabled = true;
                capsule_rbody.velocity = transform.up* 0f;
                if ((Input.GetKey(KeyCode.A))|| (Input.GetKey(KeyCode.Keypad4)))
                {
                    capsule_rbody.AddRelativeTorque(new Vector3(0f, 0f, -5f));
                }
                if ((Input.GetKey(KeyCode.D))|| (Input.GetKey(KeyCode.Keypad6)))
                {
                    capsule_rbody.AddRelativeTorque(new Vector3(0f, 0f, 5f));
                }
                if ((Input.GetKey(KeyCode.W))|| (Input.GetKey(KeyCode.Keypad8)))
                {
                    capsule_rbody.AddRelativeTorque(new Vector3(-5f, 0f, 0f));
                }
                if ((Input.GetKey(KeyCode.S))|| (Input.GetKey(KeyCode.Keypad2)))
                {
                    capsule_rbody.AddRelativeTorque(new Vector3(5f, 0f, 0f));
                }
                if ((Input.GetKey(KeyCode.Space)))
                {
                    manual_control = false;
                    manual_flag = false;
                    image_text.enabled = false;
                }
            }
            else
            {
                capsule_rbody.AddRelativeTorque(new Vector3(torque_direction_x * torque_value, torque_direction_y * torque_value , torque_direction_z* torque_value));
                reward -= 3f;
                AddReward(reward);
                capsule_rbody.velocity = transform.up* 0f;
            }
        }
        else
        {
            if (!emergency_stop)
            {
                capsule_rbody.AddRelativeTorque(new Vector3(torque_direction_x * torque_value, torque_direction_y * torque_value , torque_direction_z* torque_value));
                capsule_rbody.velocity = -transform.up* translation_speed;
                reward += 3f-3*distance;
                AddReward(reward);
                count2 = 0;
                manual_flag = false;
                distance_flag = false;
            }
        }
        
        
        // int direction_value = direction.Get_i_value();
        // // Debug.Log("direction value" + direction_value);
        // // int index_difference = new_index - index_centerline;

        // if (direction_value >= 0)
        // {
        //     reward = +0f;
        //     // Debug.Log("Here is the reward");
        //     AddReward(reward);
        // }
        // else
        // {
        //     reward = -50f;
        //     // Debug.Log("Here is the reward");
        //     AddReward(reward);
        // }

        // index_centerline = new_index;
        // Debug.Log(index_difference);
        // Fell off platform
        bool coinflag = cointouch.CC_reached;
        // Debug.Log("coinflag"+coinflag);
        if (coinflag)
        {   
            Debug.Log("coin reached finally");
            reward += 5000;
            AddReward(reward);
            EndEpisode();
        }

        // if ((this.transform.position.x < CAPSULE_XLIMIT_LOW) || (this.transform.position.x > CAPSULE_XLIMIT_UP) || (this.transform.position.y < CAPSULE_YLIMIT_LOW) || (this.transform.position.y > CAPSULE_YLIMIT_UP) || (this.transform.position.z < CAPSULE_ZLIMIT_LOW))
        // // if ( (this.transform.position.x < CAPSULE_XLIMIT_LOW) || (this.transform.position.x > CAPSULE_XLIMIT_UP) || (this.transform.position.y < CAPSULE_YLIMIT_LOW) || (this.transform.position.y > CAPSULE_YLIMIT_UP))
        // {  
        //     fellOff = true;
        //     //AddReward(-0.3f);
        //     reward -= 2000f;
        //     AddReward(reward);
        //     // Debug.Log("Negative Reward");
        //     EndEpisode();
        // }
        if (Input.GetKey(KeyCode.O)){emergency_stop = true;}
        
        if (emergency_stop == true)
        {
            capsule_rbody.velocity = -transform.up* 0f;
            image_text.enabled = true;
            if ((Input.GetKey(KeyCode.A))|| (Input.GetKey(KeyCode.Keypad4)))
            {
                capsule_rbody.AddRelativeTorque(new Vector3(0f, 0f, -5f));
            }
            if ((Input.GetKey(KeyCode.D))|| (Input.GetKey(KeyCode.Keypad6)))
            {
                capsule_rbody.AddRelativeTorque(new Vector3(0f, 0f, 5f));
            }
            if ((Input.GetKey(KeyCode.W))|| (Input.GetKey(KeyCode.Keypad8)))
            {
                capsule_rbody.AddRelativeTorque(new Vector3(-5f, 0f, 0f));
            }
            if ((Input.GetKey(KeyCode.S))|| (Input.GetKey(KeyCode.Keypad2)))
            {
                capsule_rbody.AddRelativeTorque(new Vector3(5f, 0f, 0f));
            }
            if ((Input.GetKey(KeyCode.Z))|| (Input.GetKey(KeyCode.Keypad0)))
            {
                capsule_rbody.velocity = -transform.up* translation_speed;
            }
            if ((Input.GetKey(KeyCode.Space)))
            {
                // manual_control = false;
                // manual_flag = false;
                emergency_stop=false;
                image_text.enabled = false;
            }
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = -1;
        actionsOut[1] = -1;
        actionsOut[2] = -1;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
        actionsOut[0] = -1;
        //Debug.Log("dhaflafas");
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
        actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
        actionsOut[1] = -1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
        actionsOut[1] = 1;
        }
    }
}