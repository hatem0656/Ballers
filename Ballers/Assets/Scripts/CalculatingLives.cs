using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculatingLives : NetworkBehaviour
{
    private int Lives = 5;
    private Rigidbody rb;
    private float timer = 0.0f;
    Transform[] RespwanPoints;
    Image[] Hearts ;
   // public GameObject lives; 

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        RespwanPoints = GameObject.Find("RespwanPoints").GetComponentsInChildren<Transform>();
      //  Hearts = lives.GetComponentsInChildren<Image>();
    }


    void Update()
    {
        if (hasAuthority)
        {
            if (rb.position.y < 7)
            {
                DecreaseLive();
            }
        }
    }
    void DecreaseLive()
    {

        if ((Time.time - timer) > 3)
        {
          //  GameObject.Destroy(Hearts[Lives]);
            Lives--;
            Debug.Log(Lives);
            this.transform.position = RespwanPoints[Random.Range(0, RespwanPoints.Length)].position;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;

        }
        timer = Time.time;
        if (Lives == 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
