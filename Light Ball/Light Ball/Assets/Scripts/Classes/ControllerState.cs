using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ControllerState{

    public enum InputState
    {
        Controller,
        Keyboard
    }

    public InputState inputDevice;

    public bool IsVisible { get; set; }

    public bool EnteredSphere
    {
        get { return IsVisible; }
    }
}
