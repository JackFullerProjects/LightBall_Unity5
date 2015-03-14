using UnityEngine;
using System.Collections;

public class ControllerState : MonoBehaviour {

    public enum InputState
    {
        Controller,
        Keyboard
    }

    public InputState inputDevice;
}
