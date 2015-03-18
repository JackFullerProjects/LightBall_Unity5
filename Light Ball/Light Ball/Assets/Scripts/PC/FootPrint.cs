using UnityEngine;
using System.Collections;

public class FootPrint : Photon.MonoBehaviour {

    Rigidbody playerRigidbody;
    public float DropTime;
    private float droptime_Time;
	// Use this for initialization
	void Start ()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        droptime_Time = DropTime + Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {

        DetectMovement();
	
	}

    private void DetectMovement()
    {
        if (playerRigidbody.velocity.magnitude > 0)
        {
            if (droptime_Time < Time.realtimeSinceStartup)
            {
                float yCorrection = transform.localScale.y / 2;
                Vector3 rotationFix = new Vector3(270f, 0f, 0f);
                Vector3 footPrintPos = transform.position;
                footPrintPos.y -= yCorrection;

                GameObject clone = PhotonNetwork.Instantiate("BlueFootprint", footPrintPos,
                                                                transform.rotation,
                                                                0) as GameObject;
                clone.transform.eulerAngles = rotationFix;
                droptime_Time = DropTime + Time.realtimeSinceStartup;
            }
        }
        else
        {
            droptime_Time = DropTime + Time.realtimeSinceStartup;
        }
    }

}
