using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTrap : NetworkBehaviour
{    
    public Rigidbody rb;
    public Vector3 Angularspeed = new Vector3() ;
    public float speed = 10.0f , directionFact = 2   ; 
    void Start()
    {  
        rb= this.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition; 
        rb.maxAngularVelocity = Mathf.Deg2Rad * 500 ; 
    }

    
    void Update()
    {      
            Angularspeed = rb.angularVelocity;
            rb.angularVelocity = new Vector3(0, speed * Mathf.Deg2Rad, 0);
            
            speed = speed + directionFact;
            if (speed > (rb.maxAngularVelocity * Mathf.Rad2Deg) || speed < -(rb.maxAngularVelocity * Mathf.Rad2Deg))
            {
                rb.angularVelocity = Vector3.zero;
                speed = -10.0f;
                directionFact = -directionFact;



            }
        
    }
 
}
