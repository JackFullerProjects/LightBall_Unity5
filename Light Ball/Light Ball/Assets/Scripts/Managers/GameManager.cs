using UnityEngine;
using System.Collections;

public class GameManager : Photon.MonoBehaviour {

    public static int BlueScore = 0;
    public static int RedScore = 0;
    public static float Timer = 600f;

    private bool RedWins;
    private bool BlueWins;

    private float cooldown = 0.5f;
    private float cooldownTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (cooldownTime < Time.realtimeSinceStartup)
            {
                GetComponent<PhotonView>().RPC("SyncScore", PhotonTargets.AllBuffered, GameManager.BlueScore, GameManager.RedScore);
                cooldownTime = Time.realtimeSinceStartup + cooldown;
            }

            if(Timer > 0)
                GetComponent<PhotonView>().RPC("IncreaseTimer", PhotonTargets.AllBuffered, Timer);

            if(Timer <= 0)
            {
                if (BlueScore == RedScore)
                {
                    BlueWins = true;
                    RedWins = true;
                }
                else if (BlueScore > RedScore)
                {
                    BlueWins = true;
                }
                else
                {
                    RedWins = true;
                }
            }

            if(RedScore == 50)
            {
                RedWins = true;
            }
            else if(BlueScore == 50)
            {
                BlueWins = true;
            }
        }
	}



    private void CalculateWinner()
    {
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
    public void SyncScore(int _blueScore, int _redScore)
    {
        GameManager.BlueScore = _blueScore;
        GameManager.RedScore = _redScore;
    }

    [RPC]
    public void IncreaseTimer(float gameTime)
    {
        gameTime -= Time.deltaTime;
        Timer = gameTime;
    }

   

    void OnGUI()
    {
        float rx = Screen.width / 1980f;
        float ry = Screen.height / 1080f;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));
        GUI.color = Color.yellow;
        if (BlueWins && RedWins)
        {
            GUI.Box(new Rect(690, 600, 600, 50), "DRAW");
        }

        if (RedWins && !BlueWins)
        {
            GUI.Box(new Rect(690, 600, 600, 50), "RED TEAM WINS");
        }

        if (BlueWins && !RedWins)
        {
            GUI.Box(new Rect(690, 600, 600, 50), "BLUE TEAM WINS");
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
