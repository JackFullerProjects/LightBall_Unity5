using UnityEngine;
using System.Collections;

public class GUIManager : Photon.MonoBehaviour {

    float OriginalWidth = 1980f;
    float OriginalHeight = 1080f;

    Player playerScript;

	// Use this for initialization
	void Start () {

        playerScript = GetComponent<Player>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {

        float rx = Screen.width / OriginalWidth;
        float ry = Screen.height / OriginalHeight;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(rx, ry, 1));

        GUI.contentColor = Color.green;
        string minutes = Mathf.Floor(GameManager.Timer / 60).ToString("00");
        string seconds = (GameManager.Timer % 60).ToString("00");
        GUI.Box(new Rect(690, 50, 600, 50), "Red Score: " + GameManager.RedScore + " / 50" + "   Blue Score: " + GameManager.BlueScore + "/ 50" + "    Time: " + minutes + ":" + seconds);

        GUI.contentColor = Color.yellow;
        GUI.Box(new Rect(790,1000, 400, 50), "Health: " + playerScript.playerData.health);
        GUI.Box(new Rect(1800, 900, 150, 50), "Ammo: " + playerScript.destructionModuleClass.ShotsInClip + " / " + playerScript.destructionModuleClass.Ammo);
    }
}


 //GUI.contentColor = Color.green;
 //       GUI.Box(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 400, 400, 50), "Red Score: " + GameManager.RedScore + " / 50" + "   Blue Score: " + GameManager.BlueScore + "/ 50");

 //       GUI.contentColor = Color.yellow;
 //       GUI.Box(new Rect((Screen.width / 2) - 200, (Screen.height / 2) + 370, 400, 50), "Health: " + playerScript.playerData.health + "     Des Ammo: " + playerScript.destructionModuleClass.Ammo + "    Impair Ammo: " + playerScript.impairmentModuleClass.Ammo);