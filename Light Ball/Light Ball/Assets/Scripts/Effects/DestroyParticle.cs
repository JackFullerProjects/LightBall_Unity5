using UnityEngine;
using System.Collections;

public class DestroyParticle : Photon.MonoBehaviour {

	
	// Update is called once per frame
	void Update () 
    {
        if (GetComponent<PhotonView>().instantiationId == 0)
        {
            if (!GetComponentInChildren<ParticleSystem>().isPlaying)
            Destroy(gameObject);
        }
        else
        {
            if (PhotonNetwork.isMasterClient)
            {
                if (!GetComponentInChildren<ParticleSystem>().isPlaying)
                    PhotonNetwork.Destroy(gameObject);
            }
        }
       
            
	}
}
