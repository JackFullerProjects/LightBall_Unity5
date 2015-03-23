using UnityEngine;
using System.Collections;

public class DestroyObject : Photon.MonoBehaviour {

    public float DestroyTime;
    public bool isForceField;
    void Update()
    {
        DestroyTime -= Time.deltaTime;

        if (DestroyTime <= 0 && GetComponent<PhotonView>().isMine)
        {
            if (isForceField)
            {
                ForceFieldPreDestroy();
            }
            this.photonView.RPC("Destroy", PhotonTargets.AllBuffered);
        }
    }

    private void ForceFieldPreDestroy()
    {
        var forceFieldScript = GetComponent<ForceFieldHealth>();

        for(int i = 0; i < forceFieldScript.ForceFieldClass.PlayersInField.Count; i ++)
        {
            forceFieldScript.ForceFieldClass.PlayersInField[i].GetComponent<PlayerVisibility>().TurnInvisible();
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
