using UnityEngine;
using System.Collections;

public class DestroyObject : Photon.MonoBehaviour {

    public float DestroyTime;

    void Update()
    {
        DestroyTime -= Time.deltaTime;

        if (DestroyTime <= 0 && GetComponent<PhotonView>().isMine)
        {
            this.photonView.RPC("Destroy", PhotonTargets.AllBuffered);
        }
    }

   [RPC]
    public IEnumerator Destroy()
    {
        GameObject.Destroy(this.gameObject);
        yield return 0; // if you allow 1 frame to pass, the object's OnDestroy() method gets called and cleans up references.
        PhotonNetwork.UnAllocateViewID(this.photonView.viewID);
    }
}
