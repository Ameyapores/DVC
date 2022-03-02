using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL;

public class LSLcoinoutput : MonoBehaviour
{
    //[SerializeField] GameObject coinobject; 
    private float Sample;
    bool CC_reached=false;
 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float Getcoin()
    {
        
        if (CC_reached == true)
            Sample = 1;

        else
            Sample = 0;
       //Debug.Log("Sample");
        
        return Sample;
    }

    private void OnTriggerEnter(Collider other)
    {

        CC_reached = true;
        // if (other.tag == "CC"){
        //     Destroy(other.gameObject);

    }
}