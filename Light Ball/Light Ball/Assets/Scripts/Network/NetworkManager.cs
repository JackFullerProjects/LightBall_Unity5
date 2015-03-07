using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera lobbyCamera;

	[SerializeField] GameObject serverWindow;
	[SerializeField] InputField username;
	[SerializeField] InputField roomName;
	[SerializeField] InputField roomList;


	public string VERSION = "v0.0.1";

	private GameObject player;

	// Use this for initialization
	void Start () {

		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings(VERSION);
		StartCoroutine("UpdateConnectionString");

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
		Debug.Log(rooms.Length);

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
		StartSpawnProcess(1f);
	}

	void StartSpawnProcess(float _respawnTime)
	{
		lobbyCamera.enabled = true;
		StartCoroutine("SpawnPlayer", _respawnTime);
	}




	IEnumerator SpawnPlayer(float _respawnTime)
	{
		yield return new WaitForSeconds(_respawnTime);

		int spawnIndex = Random.Range(0, spawnPoints.Length);

		player = PhotonNetwork.Instantiate("Player", spawnPoints[spawnIndex].position,
		                                   			 spawnPoints[spawnIndex].rotation,
		                                             0);

		lobbyCamera.enabled = false;

	}

}
