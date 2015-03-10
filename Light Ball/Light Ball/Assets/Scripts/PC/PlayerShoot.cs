using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            var player = GetComponent<Player>();
            player.Fire();
        }
	
	}
}
