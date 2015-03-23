using UnityEngine;
using System.Collections;

public class ForceFieldHealth : Photon.MonoBehaviour {

    public float Health = 100f;
    public PlayersInForceField ForceFieldClass;

    [RPC]
    public void TakeDamage(int _damage)
    {
        Health -= _damage;

        if (Health <= 0)
        {
            ForceFieldPreDestroy();
            this.photonView.RPC("Destroy", PhotonTargets.AllBuffered);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public void ForceFieldPreDestroy()
    {
        var forceFieldScript = GetComponent<ForceFieldHealth>();

        for (int i = 0; i < forceFieldScript.ForceFieldClass.PlayersInField.Count; i++)
        {
            forceFieldScript.ForceFieldClass.PlayersInField[i].GetComponent<PlayerVisibility>().TurnInvisible();
        }
    }
}
