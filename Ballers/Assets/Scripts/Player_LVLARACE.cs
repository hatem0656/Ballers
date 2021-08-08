using Mirror;
using System;
using UnityEngine;


public class Player_LVLARACE : NetworkBehaviour {

    [SyncVar] public Vector3 movement = new Vector3();
    [SyncVar] public Vector3 CollisionForce = new Vector3();
    [SyncVar] public string playerName = "Player";
    [SyncVar] public bool isReady = false;
    public Vector3 position_checkpoint = Vector3.zero;
    public VariableJoystick variableJoy;
    //public GameObject joyst;
    public Rigidbody rb;
    int selectedRespawn = 1;
    Transform[] RespawnPoints;
    public float speed = 15.0f;
    public TextMesh nameText;
    public String forceType;
    [SyncVar] public bool Finish_Line_Crossed = false;


    void Start() {
        rb = this.GetComponent<Rigidbody>();
        //GameObject.Find("VariableJoystick").SetActive(false);
        //joyst.SetActive(false);
        RespawnPoints = GameObject.Find("RespawnPoss").GetComponentsInChildren<Transform>();
        GetNearestRespawnPoint();
        //gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
    }

    // Update is called once per frame
    void LateUpdate() {

        nameText.text = playerName;
        nameText.transform.rotation = Camera.main.transform.rotation;
        nameText.transform.position = gameObject.transform.position + new Vector3(0, 1, 0);

    }

    private void FixedUpdate() {



        if (hasAuthority) {

            variableJoy = FindObjectOfType<VariableJoystick>();

            movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#if UNITY_ANDROID || UNITY_IOS
            movement = new Vector3(variableJoy.Horizontal, 0, variableJoy.Vertical);
#endif
        }

        if (hasAuthority && isReady == true) {
            moveCharacter(movement);
            if (gameObject.transform.position.z > position_checkpoint.z) {
                position_checkpoint = gameObject.transform.position;
            }
            if (-10 > gameObject.transform.position.y) {
                Respawn();
            }

        }
    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            CollisionForce = collision.relativeVelocity;
            forceType = "Impulse";
        }
        else if (collision.gameObject.tag == "Bouncy_Trap") {
            CollisionForce = collision.relativeVelocity * 1.2f;
            forceType = "Impulse";
        }
    }


    void moveCharacter(Vector3 direction) {
        if (forceType == "Impulse") {
            rb.AddForce((CollisionForce * 75) * Time.deltaTime, ForceMode.Impulse);
            forceType = "any";
        }
        else {
            rb.AddForce(direction * speed);
            //rb.AddTorque(direction *10000);
        }
    }

    void Respawn() {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        GetNearestRespawnPoint();
        gameObject.transform.position = RespawnPoints[selectedRespawn].position;
    }

    void GetNearestRespawnPoint() {
        float nearest = Vector3.Distance(position_checkpoint, RespawnPoints[1].position);
        for (int i = 1; i < RespawnPoints.Length; i++) {
            if (nearest > Vector3.Distance(position_checkpoint, RespawnPoints[i].position)) {
                nearest = Vector3.Distance(position_checkpoint, RespawnPoints[i].position);
                if (position_checkpoint.z > RespawnPoints[i].position.z) {
                    selectedRespawn = i;
                }
            }
        }
    }

    public void SendReadyToServer(string playername) {
        if (!isLocalPlayer)
            return;
        nameText.color = Color.green;
        CmdReady(playername);
    }
    
    public void OnTriggerEnter(Collider other) {  // Collider of type Is Trigger at the finish line
        if (other.gameObject.tag == "Finish") {

            if (isLocalPlayer) {
                // if this is a local player it will execute the following function which sends a command to the server telling him i crossed the finish line
                FinishLineCross();
                //Debug.Log("Winner: " + playerName + "  Passed finish: " + Finish_Line_Crossed);
            }
        }
    }

    [Command]
    void CmdReady(string playername) {
        if (string.IsNullOrEmpty(playername)) {
            playerName = "PLAYER" + UnityEngine.Random.Range(1, 99);
        }
        else {
            playerName = playername;
        }

        isReady = true;
    }
    [Command]
    void FinishLineCross() {
        Finish_Line_Crossed = true;
    }
/*    [Command]
    void CmdWinner(string playername) {
        CmdAnnounceWinner(playername);
    }

    [ClientRpc]
    void CmdAnnounceWinner(string Winnername) {
        Debug.Log("Winner: " + Winnername);
    }*/

}
