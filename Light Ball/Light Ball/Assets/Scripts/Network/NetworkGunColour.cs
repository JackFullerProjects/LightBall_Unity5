using UnityEngine;
using System.Collections;

public class NetworkGunColour : MonoBehaviour {

    public  Material[] GunColour;
    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        if(!pv.isMine)
        {

        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(GetComponent<MeshRenderer>().materials);
        }
        else
        {
            GunColour = (Material[])stream.ReceiveNext();
        }
    }
}
