using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {

    public int HP = 100;
    public int Armour = 100;

    [RPC]
    public void TakeDamage(int _healthDamage)
    {
        HP -= _healthDamage;

        if (GetComponent<PhotonView>().isMine)
        {
            //Update GUI
        }

        if (HP <= 0)
        {
            HP = 100;

            if (GetComponent<PhotonView>().isMine)
            {
               //update GUI
            }
            LevelManager.RespawnPlayer(gameObject);
        }
        
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    void OnGUI()
    {

    }

}
