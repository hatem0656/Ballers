using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    [SyncVar] public Vector3 movement = new Vector3();
    [SyncVar] public Vector3 CollisionVelocity;
   // public VariableJoystick variableJoy;
    public GameObject joyst;
    public Rigidbody rb;
    public float SpeedingFactor = 100;
    public String forceType;
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        //GameObject.Find("VariableJoystick").SetActive(false);
       // joyst.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (hasAuthority)
        {

            /* if (Input.GetKeyDown(KeyCode.Space)) {

                //transform.Translate(movement);
                CmdMove();
            } */
            //variableJoy = FindObjectOfType<VariableJoystick>();

            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#if UNITY_ANDROID || UNITY_IOS
            movement = new Vector3(variableJoy.Horizontal, 0, variableJoy.Vertical);
#endif
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            CollisionVelocity = collision.relativeVelocity;

            forceType = "Impulse";
        }
    }
    public void OnCollisionStay(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezePositionY;

    }

    void moveCharacter()
    {
        if (forceType == "Impulse")
        {
            
            rb.AddForce((CollisionVelocity * 50) * Time.deltaTime, ForceMode.Impulse);
            forceType = "Force";
        }
        else
        {
            rb.AddForce(movement * SpeedingFactor, ForceMode.Force);

        }


    }
    private void FixedUpdate()
    {
        if (hasAuthority)
        {
            moveCharacter();
        }

    }
}
