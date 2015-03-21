using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {

    public int HP = 100;
    public int Armour = 100;

    [RPC]
    public void TakeDamage(int _healthDamage, int _armourDamage)
    {
        if (Armour > 0)
        {
            Armour -= _armourDamage;
            if(GetComponent<PhotonView>().isMine)
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>().Armour.text = "" + Armour;
            return;
        }

        HP -= _healthDamage;

        if (GetComponent<PhotonView>().isMine)
            GameObject.Find("NetworkManager").GetComponent<NetworkManager>().Health.text = "" + HP;

        if (HP <= 0)
        {
            Armour = 100;
            HP = 100;

            if (GetComponent<PhotonView>().isMine)
            {
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>().Armour.text = "" + Armour;
                GameObject.Find("NetworkManager").GetComponent<NetworkManager>().Health.text = "" + HP;
            }
            LevelManager.RespawnPlayer(gameObject);
        }
        
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
