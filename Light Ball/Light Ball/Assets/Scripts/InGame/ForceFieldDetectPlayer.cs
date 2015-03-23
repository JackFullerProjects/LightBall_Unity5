using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForceFieldDetectPlayer : Photon.MonoBehaviour {


    void OnTriggerStay(Collider other)
    {
        var playerVisibleScript = other.GetComponent<PlayerVisibility>();

        if (!playerVisibleScript)
            return;

        var forceFieldClass = transform.parent.GetComponent<ForceFieldHealth>();
        playerVisibleScript.ForceFieldInside = gameObject;
        playerVisibleScript.TurnVisible();

        for (int i = 0; i < forceFieldClass.ForceFieldClass.PlayersInField.Count; i++)
        {
            if (other.gameObject == forceFieldClass.ForceFieldClass.PlayersInField[i].gameObject)
                return;
        }

        forceFieldClass.ForceFieldClass.PlayersInField.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        var playerVisibleScript = other.GetComponent<PlayerVisibility>();

        if (!playerVisibleScript)
            return;

        var forceFieldClass = transform.parent.GetComponent<ForceFieldHealth>();
        playerVisibleScript.ForceFieldInside = null;
        playerVisibleScript.TurnInvisible();

        for (int i = 0; i < forceFieldClass.ForceFieldClass.PlayersInField.Count; i++)
        {
            if (other.gameObject == forceFieldClass.ForceFieldClass.PlayersInField[i].gameObject)
                forceFieldClass.ForceFieldClass.PlayersInField.Remove(other.gameObject);
        }

        
    }
}
