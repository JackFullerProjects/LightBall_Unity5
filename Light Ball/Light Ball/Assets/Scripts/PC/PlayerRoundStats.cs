using UnityEngine;
using System.Collections;

public class PlayerRoundStats : Photon.MonoBehaviour {

    public PlayerPerformanceClass PlayerPerformance;

    [RPC]
    public void IncreaseKills(PhotonMessageInfo info)
    {
          PlayerPerformance.Kills++;
    }

    [RPC]
    public void IncreaseDeaths(PhotonMessageInfo info)
    {
         PlayerPerformance.Deaths++;
    }

    [RPC]
    public void IncreaseAssists(PhotonMessageInfo info)
    {
         PlayerPerformance.Assists++;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
