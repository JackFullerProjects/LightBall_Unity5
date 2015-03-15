using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {


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
