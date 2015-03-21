using UnityEngine;
using System.Collections;

public class ForceFieldHealth : Photon.MonoBehaviour {

    public float Health = 100f;


    [RPC]
    public void TakeDamage(int _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
