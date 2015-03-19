using UnityEngine;
using System.Collections;

public class DestroyObject : Photon.MonoBehaviour {

    public float DestroyTime;

    void Start()
    {
        PhotonView.Destroy(gameObject, DestroyTime);
    }
}
