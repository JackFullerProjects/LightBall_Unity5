using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera lobbyCamera;

	[SerializeField] GameObject serverWindow;
	[SerializeField] InputField username;
	[SerializeField] InputField roomName;
	[SerializeField] InputField roomList;
    [SerializeField] Text PlayerCount;
    public Text Armour;
    public Text Health;


    [SerializeField] GameObject playerHUD;


	public string VERSION = "v0.0.1";

	private GameObject player;

	void Awake()
	{
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () 
	{

		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings(VERSION);
		StartCoroutine("UpdateConnectionString");

	}

    void Update()
    {
        PlayerCount.text = "Players In Room: " + PhotonNetwork.playerList.Length;
    }

	public void JoinRoom()
	{
		PhotonNetwork.player.name = username.text;
		RoomOptions roomOption = new RoomOptions(){ isVisible = true, maxPlayers = 4 };
		PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOption, TypedLobby.Default);
	}

	// Update is called once per frame
	IEnumerator UpdateConnectionString () 
	{
		while(true)
		{
			connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
			yield return null;
		}
	
	}

	void OnJoinedLobby()
	{
		serverWindow.SetActive(true);

	}

	void OnReceivedRoomListUpdate()
	{
		RoomInfo[] rooms = PhotonNetwork.GetRoomList();

		foreach(RoomInfo room in rooms)
		{
			roomList.text += room.name + "\n";
		}
	}

	void OnJoinedRoom()
	{
		StopCoroutine("UpdateConnectionString");
		connectionText.text = "";
		serverWindow.SetActive(false);
        GameObject.Find("NetworkManager").GetComponent<ChatBox>().AddLine("New Player Has Joined The Game");
		StartSpawnProcess(1f);
	}

    void OnLeaveRoom()
    {
        Debug.Log("Left");
    }

	void StartSpawnProcess(float _respawnTime)
	{
		lobbyCamera.enabled = true;
		StartCoroutine("SpawnPlayer", _respawnTime);
	}




	IEnumerator SpawnPlayer(float _respawnTime)
	{
		yield return new WaitForSeconds(_respawnTime);

		player = PhotonNetwork.Instantiate("PlayerRB", transform.position,
		                                   			 transform.rotation,
		                                             0);

        playerHUD.SetActive(true);

		lobbyCamera.enabled = false;
    }

    void OnGUI()
    {
        foreach (var teamName in PunTeams.PlayersPerTeam.Keys)
        {
            GUILayout.Label("Team: " + teamName.ToString());
            List<PhotonPlayer> teamPlayers = PunTeams.PlayersPerTeam[teamName];

            foreach (PhotonPlayer player in teamPlayers)
            {
                GUILayout.Label("  " + player.ToStringFull());
            }
        }
    }

}
