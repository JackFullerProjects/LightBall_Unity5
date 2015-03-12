using UnityEngine;
using System.Collections;

public class StopOnCollision : MonoBehaviour {

    void OnCollisionEnter(Collision obj)
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<SphereCollider>().enabled = false;
    }
}
