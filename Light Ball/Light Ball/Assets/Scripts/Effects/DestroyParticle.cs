using UnityEngine;
using System.Collections;

public class DestroyParticle : MonoBehaviour {

	
	// Update is called once per frame
	void Update () 
    {
        if (!GetComponentInChildren<ParticleSystem>().isPlaying)
            Destroy(gameObject);
	}
}
