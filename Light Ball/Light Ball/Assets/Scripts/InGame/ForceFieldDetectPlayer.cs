using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceFieldDetectPlayer : Photon.MonoBehaviour {

    public List<GameObject> playersInField = new List<GameObject>();

    void OnTriggerStay(Collider other)
    {
        var playerVisibleScript = other.GetComponent<PlayerVisibility>();

        if (!playerVisibleScript)
            return;

    
        playerVisibleScript.ForceFieldInside = gameObject;
        playerVisibleScript.TurnVisible();

        for (int i = 0; i < playersInField.Count; i++)
        {
            if (other.gameObject == playersInField[i].gameObject)
                return;
        }

        playersInField.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        var playerVisibleScript = other.GetComponent<PlayerVisibility>();

        if (!playerVisibleScript)
            return;

        playerVisibleScript.ForceFieldInside = null;
        playerVisibleScript.TurnInvisible();
    }
}
