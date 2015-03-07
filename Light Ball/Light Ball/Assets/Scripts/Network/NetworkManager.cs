using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {

	[SerializeField] Text connectionText;
	[SerializeField] Transform[] spawnPoints;
	[SerializeField] Camera lobbyCamera;

	public string VERSION = "v0.0.1";

	private GameObject player;

	// Use this for initialization
	void Start () {

		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings(VERSION);
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		connectionText.text = PhotonNetwork.connectionStateDetailed.ToString();
	
	}

	void OnJoinedLobby()
	{
		RoomOptions roomOption = new RoomOptions(){ isVisible = true, maxPlayers = 4 };
		PhotonNetwork.JoinOrCreateRoom("JackLovesTheD", roomOption, TypedLobby.Default);
	}

	void OnJoinedRoom()
	{
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
