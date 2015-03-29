using UnityEngine;
using System.Collections;

public class GameManager : Photon.MonoBehaviour {

    public static int BlueScore = 0;
    public static int RedScore = 0;
    public static float Timer = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (PhotonNetwork.isMasterClient)
        {
            GetComponent<PhotonView>().RPC("IncreaseTimer", PhotonTargets.All, Timer);
        }
	}

    [RPC]
    public void IncreaseScore(int teamRef)
    {
        if (teamRef == 1)
            GameManager.RedScore++;
        else if (teamRef == 2)
            GameManager.BlueScore++;
        else
            Debug.Log("Error: Team Not Assigned or Recognised");
    }

    [RPC]
    public void IncreaseTimer(float gameTime)
    {
        gameTime += Time.deltaTime;
        Timer = gameTime;
    }



    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
