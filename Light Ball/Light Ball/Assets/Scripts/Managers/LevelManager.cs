using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class LevelManager : MonoBehaviour {

    [System.Serializable]
    public class Teams
    {
        public string teamName;
        public int score;
        public int tickets;
        public List<GameObject> playersOnTeam = new List<GameObject>();
    }

    public List<Teams> li_Teams = new List<Teams>();




    public static void RespawnPlayer(GameObject _PlayerToRespawn)
    {
        GameObject[] spawns = GameObject.FindGameObjectsWithTag("Respawn");
        _PlayerToRespawn.transform.position = spawns[ChooseSpawn(0, spawns.Length)].transform.position;
    }

    public static int ChooseSpawn(int min, int max)
    {
        return Random.Range(min, max);
    }
}
