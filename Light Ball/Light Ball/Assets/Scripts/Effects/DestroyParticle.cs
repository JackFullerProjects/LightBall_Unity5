using UnityEngine;
using System.Collections;

public class DestroyParticle : Photon.MonoBehaviour {

	
	// Update is called once per frame
	void Update () 
    {
        if (!GetComponentInChildren<ParticleSystem>().isPlaying)
            PhotonNetwork.Destroy(gameObject);
            
	}
}
