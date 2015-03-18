using UnityEngine;
using System.Collections;

public class FootPrint : Photon.MonoBehaviour {

    Rigidbody playerRigidbody;
    public float DropTime;
    private float droptime_Time;

    private float distanceToGround;
	// Use this for initialization
	void Start ()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        droptime_Time = DropTime + Time.realtimeSinceStartup;

        distanceToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {

        DetectMovement();
	
	}

    private void DetectMovement()
    {
        if (playerRigidbody.velocity.magnitude > 0 && IsGrounded())
        {
            if (droptime_Time < Time.realtimeSinceStartup)
            {
                float yCorrection = transform.localScale.y / 2;
                Vector3 rotationFix = new Vector3(270f, 0f, 0f);
                Vector3 footPrintPos = transform.position + transform.forward + Vector3.up * .14f;
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

    private bool IsGrounded()
    {
         return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }
 }
