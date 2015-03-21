using UnityEngine;
using System.Collections;

public class PlayerVisibility : MonoBehaviour {


    public PlayerState playerState;
    public GameObject[] GunParts;
    public GameObject PlayerMesh;
    public GameObject ForceFieldInside;

    private PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (ForceFieldInside == null)
            TurnInvisible();
    }

    public void TurnVisible()
    {
        playerState.IsVisible = true;
        PlayerMesh.SetActive(true);

        if (!pv.isMine)
        {
            for (int i = 0; i < GunParts.Length; i++)
                GunParts[i].GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void TurnInvisible()
    {
        playerState.IsVisible = false;
        PlayerMesh.SetActive(false);

        if (!pv.isMine)
        {
            for (int i = 0; i < GunParts.Length; i++)
                GunParts[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
