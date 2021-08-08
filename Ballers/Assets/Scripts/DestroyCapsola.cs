using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCapsola : NetworkBehaviour
{
    [ServerCallback]
    void Update()
    {
        
       if (gameObject.transform.position.y < -10f) {           
            NetworkServer.Destroy(gameObject);
            GameObject.Find("SceneManager").GetComponent<SpawnCapsule>().CapsuleNo--;
        }
        else {
            StartCoroutine("DestroyIfStuck",15f);
        }
    }


    IEnumerator DestroyIfStuck(float time) {

        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(gameObject);
        GameObject.Find("SceneManager").GetComponent<SpawnCapsule>().CapsuleNo--;

    }
}
