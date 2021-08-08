using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCannonBall : NetworkBehaviour
{
    public GameObject CannonBallPrefab;
    public GameObject CannonsPositionList;
    public Transform[] CannonsPostitions;
    public bool hasspawned = false;
    public Vector3 Force = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        CannonsPostitions = CannonsPositionList.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    [ServerCallback]
    void FixedUpdate() {
        if (hasspawned == false) {
            SpawnCannonBallRandomly();
        }

    }


    void SpawnCannonBallRandomly() {
        StartCoroutine(SpawnCannonBallZ(Random.Range(0.5f, 1.3f)));
    }

    IEnumerator SpawnCannonBallZ(float time) {
            int CPOS = Random.Range(1, CannonsPostitions.Length);
            GameObject CannonBall = Instantiate(CannonBallPrefab, CannonsPostitions[CPOS].position, Quaternion.identity);
            switch (CPOS) {
            case 1:
                Force = new Vector3(0.4f, 0.03f, -1);
                break;
            case 2:
                Force = new Vector3(-0.4f, 0.03f, -1);
                break;
            case 3:
                Force = new Vector3(0.2f, 0.02f, -1);
                break;
            case 4:
                Force = new Vector3(-0.2f, 0.02f, -1);
                break;
            case 5:
                Force = new Vector3(0f, -0.01f, -1);
                break;
        }
            NetworkServer.Spawn(CannonBall);

        hasspawned = true;
        yield return new WaitForSeconds(time);
        hasspawned = false;
    }

}// dir for sp 4 -0.2,0.02,-1 , 0.4,0.03,-1
