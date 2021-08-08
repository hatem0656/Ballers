using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Backup : NetworkBehaviour
{
    [SyncVar] public Vector3 movement = new Vector3();
    [SyncVar] public Vector3 CollisionForce = new Vector3();
    public VariableJoystick variableJoy;
    public GameObject joyst;
    public Rigidbody rb;
    private String RespawnPos = "Start";
    public float speed = 15.0f;
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
        if (hasAuthority) {

        //    variableJoy = FindObjectOfType<VariableJoystick>();

            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#if UNITY_ANDROID || UNITY_IOS
            movement = new Vector3(variableJoy.Horizontal, 0, variableJoy.Vertical);
#endif
        }

    }

    private void FixedUpdate() {
        if (hasAuthority) {
            moveCharacter(movement);
            if (-10 > gameObject.transform.position.y) {
                Respawn();
            }

        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            //CollisionForce = collision.gameObject.GetComponent<Rigidbody>().velocity;
            //CollisionForce =   collision.gameObject.GetComponent<Rigidbody>().velocity - rb.GetComponent<Rigidbody>().velocity;
            CollisionForce = collision.relativeVelocity ;
            forceType = "Impulse";
        }
        else if (collision.gameObject.tag == "Bouncy_Trap") {
            CollisionForce = collision.relativeVelocity * 1.2f;
            forceType = "Impulse";
        }

        
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.name == "CheckPoint1") {
            RespawnPos = "CP1";
        }
    }


    void moveCharacter(Vector3 direction) {
        if (forceType == "Impulse") {
            rb.AddForce((CollisionForce * 75) * Time.deltaTime, ForceMode.Impulse);
            forceType = "any";
        }
        else {
            rb.AddForce(direction * speed);
        }
    }

    void Respawn() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        if (RespawnPos == "Start") {
            gameObject.transform.position = new Vector3(0, 1, 0);
            
        }
        else if (RespawnPos == "CP1") {
            gameObject.transform.position = GameObject.Find("CP_RespawnPos").transform.position;                 //new Vector3(4.815269f, -0.1303999f, 57.70295f);
        }
    }

}
