using UnityEngine;
using System.Collections;

public class DestroyParticle : Photon.MonoBehaviour {

	
	// Update is called once per frame
    void Update()
    {

        if (!GetComponentInChildren<ParticleSystem>().isPlaying && GetComponent<PhotonView>().isMine)
            this.photonView.RPC("Destroy", PhotonTargets.AllBuffered);
    }


 

    [RPC]
    public IEnumerator Destroy()
    {
        GameObject.Destroy(this.gameObject);
        yield return 0; // if you allow 1 frame to pass, the object's OnDestroy() method gets called and cleans up references.
        PhotonNetwork.UnAllocateViewID(this.photonView.viewID);
    }
}
