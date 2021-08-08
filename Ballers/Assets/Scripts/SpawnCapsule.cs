using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SpawnCapsule : NetworkBehaviour
{
    public GameObject CapsulePrefab;
    public int CapsuleNo = 0;
    private int CASE = 3;
    private bool hasspawned = false;
    void Start()
    {
        //Instantiate(Capsule, new Vector3(Random.Range(-4f,4f), 28f, 99.61f), new Quaternion(90,90,0,1));
        
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (CapsuleNo <= 14 && hasspawned == false) {
            SpawnCapsuleRandomly();
            CapsuleNo++;
        }

    }

    
    void SpawnCapsuleRandomly() {
        StartCoroutine(Spawncapsola(Random.Range(0.5f,1.5f)));
    }

    IEnumerator Spawncapsola(float time) {
        CASE = Random.Range(0, 10);
        if (CASE <= 5) {
            GameObject Capsule = Instantiate(CapsulePrefab, new Vector3(Random.Range(-4f, 0f), 28f, 99.61f), new Quaternion(90, 90, 0, 1));
            NetworkServer.Spawn(Capsule);
        }
        else if (CASE > 5) {
            GameObject Capsule = Instantiate(CapsulePrefab, new Vector3(Random.Range(0f, 4f), 28f, 99.61f), new Quaternion(90, 90, 0, 1));
            NetworkServer.Spawn(Capsule);
        }
        hasspawned = true;
        yield return new WaitForSeconds(time);
        hasspawned = false;
    }

}
