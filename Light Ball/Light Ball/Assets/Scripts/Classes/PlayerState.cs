using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerState{

    public bool IsVisible { get; set;}

    public bool HasEnteredForceField { get { return IsVisible; } }
}
