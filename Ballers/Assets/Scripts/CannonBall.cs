using Mirror;
using UnityEngine;

public class CannonBall : NetworkBehaviour {
    public float force = 1000;
    public Vector3 direction;
    public Rigidbody rb;
    public override void OnStartServer() {
        Invoke(nameof(DestroySelf), 5);
    }

    // set velocity for server and client. this way we don't have to sync the
    // position, because both the server and the client simulate it.
    void Start() {
        direction = GameObject.Find("SceneManager").GetComponent<SpawnCannonBall>().Force;
        rb.AddForce(direction * force);

    }

    // destroy for everyone on the server
    [Server]
    void DestroySelf() {
        NetworkServer.Destroy(gameObject);
    }
}
