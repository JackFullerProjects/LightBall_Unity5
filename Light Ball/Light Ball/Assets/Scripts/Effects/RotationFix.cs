using UnityEngine;
using System.Collections;

public class RotationFix : MonoBehaviour {

    public Vector3 Rotation;
	// Use this for initialization
	void Start () 
    {
        transform.eulerAngles = Rotation;
	}
	
}
