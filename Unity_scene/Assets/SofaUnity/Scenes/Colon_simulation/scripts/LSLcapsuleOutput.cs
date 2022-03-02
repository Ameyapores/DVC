using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;
using System;
using System.IO;
using System.Text;


public class LSLcapsuleOutput : MonoBehaviour
{
    private StreamOutlet outlet;
    [SerializeField] GameObject centroid;
    private float[] currentSample;
    private string[] sample;
    [SerializeField] GameObject coinobject;
    [SerializeField] GameObject circleobject;
    [SerializeField] GameObject colondiff;
    string path = @"C:\Users\ameya\Desktop\test.txt";

    public string StreamName = "Unity.ExampleStream";
    public string StreamType = "Unity.StreamType";
    public string StreamId = "MyStreamID-Unity1234";

    public float frequency;
    public int NumberOfChannels;

    private bool LoggingEnabled;
    private Rigidbody capsule_rbody;
    private WaitForSecondsRealtime wait;

    private image_processing centroid_pos;

    void Start()
    {
        capsule_rbody = this.GetComponent<Rigidbody>();
        centroid_pos = centroid.GetComponent<image_processing>();
        StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, NumberOfChannels, frequency, channel_format_t.cf_float32);
        XMLElement chans = streamInfo.desc().append_child("channels");
        chans.append_child("channel").append_child_value("label", "capsule_pX");
        chans.append_child("channel").append_child_value("label", "capsule_pY");
        chans.append_child("channel").append_child_value("label", "capsule_pZ");
        chans.append_child("channel").append_child_value("label", "capsule_rX");
        chans.append_child("channel").append_child_value("label", "capsule_rY");
        chans.append_child("channel").append_child_value("label", "capsule_rZ");
        chans.append_child("channel").append_child_value("label", "capsule_velocity");
        chans.append_child("channel").append_child_value("label", "capsule_angularvelocity");
        //chans.append_child("channel").append_child_value("label", "circle");
        chans.append_child("channel").append_child_value("label", "coin");
        chans.append_child("channel").append_child_value("label", "distance");
       
        outlet = new StreamOutlet(streamInfo);
        currentSample = new float[NumberOfChannels];
        string[] sample = new string[1];

        wait = new WaitForSecondsRealtime(1.0f / frequency);
        LoggingEnabled = true;
        IEnumerator logging = LogData();
        StartCoroutine(logging);
    }

    private IEnumerator LogData()
    {
        while (LoggingEnabled)
        {

            Vector3 pos = gameObject.transform.position;
            Vector3 rot = gameObject.transform.rotation.eulerAngles;

            currentSample[0] = pos.x;
            currentSample[1] = pos.y;
            currentSample[2] = pos.z;
            currentSample[3] = rot.x;
            currentSample[4] = rot.y;
            currentSample[5] = rot.z;
            currentSample[6] = capsule_rbody.velocity.magnitude;
            currentSample[7] = capsule_rbody.angularVelocity.magnitude;
            currentSample[8] = coinobject.GetComponent<LSLcoinoutput>().Getcoin();
            currentSample[9] = centroid_pos.Getdistance();
            //currentSample[9] = circleobject.GetComponent<ActivateCircle>().Getcircleoutput();

            outlet.push_sample(currentSample);
            yield return wait;
        }
    }
}
