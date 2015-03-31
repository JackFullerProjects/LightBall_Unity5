using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LevelManager : Photon.MonoBehaviour {

    public static GameObject[] blueSpawns;
    public static GameObject[] redSpawns;

    [System.Serializable]
    public class Teams
    {
        public string teamName;
        public int score;
        public int tickets;
        public List<GameObject> playersOnTeam = new List<GameObject>();
    }

    public List<Teams> li_Teams = new List<Teams>();


    void Start()
    {
        blueSpawns = GameObject.FindGameObjectsWithTag("BlueSpawn");
        redSpawns = GameObject.FindGameObjectsWithTag("RedSpawn");
    }



    public static void RespawnPlayer(GameObject _PlayerToRespawn)
    {
        Player player = _PlayerToRespawn.GetComponent<Player>();
        PhotonView gamemanager = GameObject.Find("GameManager").GetComponent<PhotonView>();
        _PlayerToRespawn.GetComponent<PhotonView>().RPC("IncreaseDeaths", PhotonTargets.All);

        if (player.team == PunTeams.Team.red)
        {
            int spawnIndex = ChooseSpawn(0, redSpawns.Length - 1);
            _PlayerToRespawn.transform.position = redSpawns[spawnIndex].transform.position;
            _PlayerToRespawn.transform.rotation = redSpawns[spawnIndex].transform.rotation;
            gamemanager.RPC("IncreaseScore", PhotonTargets.MasterClient, 2);
        }
        else if (player.team == PunTeams.Team.blue)
        {
            int spawnIndex = ChooseSpawn(0, blueSpawns.Length - 1);
            _PlayerToRespawn.transform.position = blueSpawns[spawnIndex].transform.position;
            _PlayerToRespawn.transform.rotation = redSpawns[spawnIndex].transform.rotation;
            gamemanager.RPC("IncreaseScore", PhotonTargets.MasterClient, 1);
        }
        else
        {
            Debug.Log("SPAWN ERROR: No Team Assigned");
        }
    }

    public static void FirstRespawn(GameObject _PlayerToRespawn)
    {
        Player player = _PlayerToRespawn.GetComponent<Player>();

        if (player.team == PunTeams.Team.red)
        {
            int spawnIndex = ChooseSpawn(0, redSpawns.Length - 1);
            _PlayerToRespawn.transform.position = redSpawns[spawnIndex].transform.position;
            _PlayerToRespawn.transform.rotation = redSpawns[spawnIndex].transform.rotation;
        }
        else if (player.team == PunTeams.Team.blue)
        {
            int spawnIndex = ChooseSpawn(0, blueSpawns.Length - 1);
            _PlayerToRespawn.transform.position = blueSpawns[spawnIndex].transform.position;
            _PlayerToRespawn.transform.rotation = redSpawns[spawnIndex].transform.rotation;
        }
        else
        {
            Debug.Log("SPAWN ERROR: No Team Assigned");
        }
    }
    public static int ChooseSpawn(int min, int max)
    {
        return Random.Range(min, max);
    }
}
