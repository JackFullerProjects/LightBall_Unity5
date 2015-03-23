using UnityEngine;
using System.Collections;

public class DestroyObject : Photon.MonoBehaviour {

    public float DestroyTime;

    void Update()
    {
        DestroyTime -= Time.deltaTime;

        if (DestroyTime <= 0)
        {
            if (GetComponent<PhotonView>().instantiationId == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                {
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }
    }
}
