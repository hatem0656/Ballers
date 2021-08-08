using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerArena : NetworkBehaviour
{
    [SyncVar] public Vector3 movement = new Vector3();
    [SyncVar] public Vector3 CollisionForce = new Vector3();
    [SyncVar] public string playerName = "Player";
    [SyncVar] public bool isReady = false;
    //public VariableJoystick variableJoy;
    // public GameObject joyst;
    public Rigidbody rb;
    private float speed = 25.0f;
    public String forceType;
    public TextMesh NameText;
    private Renderer render;   
    

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        render = this.GetComponent<Renderer>();
        
       
        
        //GameObject.Find("VariableJoystick").SetActive(false);
        // joyst.SetActive(false);
    }


    void Update()
    {
        if (hasAuthority)
        {
            //  variableJoy = FindObjectOfType<VariableJoystick>();
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
            //CollisionForce = collision.gameObject.GetComponent<Rigidbody>().velocity;
            //CollisionForce =   collision.gameObject.GetComponent<Rigidbody>().velocity - rb.GetComponent<Rigidbody>().velocity;
            var dirPlayer = transform.InverseTransformDirection(rb.velocity);
            var dirOpponent = transform.InverseTransformDirection(collision.gameObject.GetComponent<Rigidbody>().velocity);
           
            if (Mathf.Sign(dirPlayer.z) == Mathf.Sign(dirOpponent.z) || Mathf.Sign(dirPlayer.x) == Mathf.Sign(dirOpponent.x))
            {
                CollisionForce = -collision.relativeVelocity;
            }
            else
            {
                CollisionForce = collision.relativeVelocity;
            }
            forceType = "Impulse";
        }
        else if (collision.gameObject.tag == "Bouncy_Trap")
        {
            CollisionForce = collision.relativeVelocity * 1.2f;
            forceType = "Impulse";
        }
    }

    
    void moveCharacter()
    {
        if (forceType == "Impulse")
        {
            rb.AddForce((CollisionForce * 75) * Time.deltaTime, ForceMode.Impulse);
            forceType = "Force";
        }
        else
        {
            rb.AddForce(movement * speed);

        }


    }
    private void FixedUpdate()
    {
        if (hasAuthority && isReady)
        {
            moveCharacter();
        }

    }
    void LateUpdate()
    {
        NameText.text = playerName;
        NameText.transform.rotation = Camera.main.transform.rotation;
        NameText.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);

    }

    public void SendReadyToServer(string playername , string playercolor)
    {
        if (!isLocalPlayer)
            return;
        NameText.color = Color.green;
        CmdReady(playername , playercolor);
        
    }
  

    [Command]
    void CmdReady(string playername , string playercolor)
    {
        if (string.IsNullOrEmpty(playername))
        {
            playerName = "PLAYER" + UnityEngine.Random.Range(1, 99);
        }
        else
        {
            playerName = playername; 
        }
        Debug.Log(playercolor);
         setPlayerColor(playercolor);

      

        isReady = true;
    }
    void setPlayerColor(string playercolor)
    { 
        switch (playercolor)
        {
            case "Orange":
                render.material.color = new Color(1, 0.4980392f, 0);
                Debug.Log("ok");
                break;
            case "Red":
                render.material.color = new Color(1, 0.03310713f, 0);
                Debug.Log("okk");
                break;
            case "LightBlue":
                render.material.color = new Color(0, 0.4980392f, 1);
                break;
            case "DarkBlue":
                render.material.color = new Color(0.4941177f, 0, 1);
                break;
            case "Yellow":
                render.material.color = new Color(1, 1, 0.3411765f);
                break;
            case "Green":
                render.material.color = new Color(0, 0.7529413f, 0.3764706f);
                break;
            case "Violet":
                render.material.color = new Color(0.7529413f, 0, 0.7529413f);
                break;
            case "Purple":
                render.material.color = new Color(1, 0.3372549f, 0.6666667f);
                break;
            default:
                render.material.color = Color.white;
                break;
        }
    }

}
