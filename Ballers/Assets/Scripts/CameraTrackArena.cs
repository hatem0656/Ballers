using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraTrackArena : NetworkBehaviour {
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    private Vector3 velocity = Vector3.zero;

    //public GameObject player;

    /*    public override void OnStartLocalPlayer() {
           // Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);
        }*/


    void LateUpdate() {
      
            Vector3 DesiredPos = gameObject.transform.position + offset;
            Vector3 SmoothedPos = Vector3.SmoothDamp(Camera.main.transform.position, DesiredPos, ref velocity, 0.25f);
            Camera.main.transform.position = SmoothedPos;
        
        // gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(0, gameObject.transform.position.y, player.gameObject.transform.position.z + 90f), Time.deltaTime * 100);
        //Camera.main.transform.position = Vector3.Lerp (gameObject.GetComponent<Rigidbody>().position,new Vector3(gameObject.GetComponent<Rigidbody>().position.x, gameObject.GetComponent<Rigidbody>().position.y + 3, gameObject.GetComponent<Rigidbody>().position.z - 10f),Time.deltaTime * 500);
       // Camera.main.transform.position = new Vector3(gameObject.GetComponent<Rigidbody>().position.x, gameObject.GetComponent<Rigidbody>().position.y + disty, gameObject.GetComponent<Rigidbody>().position.z + distz);
    }
}