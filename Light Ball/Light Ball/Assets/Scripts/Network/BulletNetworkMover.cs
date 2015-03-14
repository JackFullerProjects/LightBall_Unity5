﻿using UnityEngine;
using System.Collections;

public class BulletNetworkMover : MonoBehaviour {

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;
    private Quaternion syncRotation = Quaternion.identity;
    private Rigidbody playerRigidbody;



    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;
    private PhotonView pv;

    public float FireForce;

    public void Awake()
    {
        PhotonNetwork.sendRate = 15;
        PhotonNetwork.sendRateOnSerialize = 15;

    }

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * FireForce, ForceMode.Force);
        pv = GetComponent<PhotonView>();
        latestCorrectPos = transform.position;
        onUpdatePos = transform.position;
    }

    public void LateUpdate()
    {
        if (!pv.isMine)
        {
            fraction = fraction + Time.deltaTime * 19;
            transform.localPosition = Vector3.Lerp(onUpdatePos, latestCorrectPos, fraction);    // set our pos between A and B
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        else
        {
            pv = GetComponent<PhotonView>();
            if (!pv.isMine)
            {
                // Receive latest state information
                Vector3 pos = Vector3.zero;
                Quaternion rot = Quaternion.identity;

                stream.Serialize(ref pos);
                stream.Serialize(ref rot);

                latestCorrectPos = pos;                 // save this to move towards it in FixedUpdate()
                onUpdatePos = transform.localPosition;  // we interpolate from here to latestCorrectPos
                fraction = 0;                           // reset the fraction we alreay moved. see Update()

                transform.localRotation = rot;          // this sample doesn't smooth rotation
            }
        }
    }
}