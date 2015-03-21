using UnityEngine;
using System.Collections;

public class DestroyObject : Photon.MonoBehaviour {

    public float DestroyTime;

    void Update()
    {
        DestroyTime -= Time.deltaTime;

        if(DestroyTime <= 0)
            PhotonNetwork.Destroy(gameObject);
    }
}
