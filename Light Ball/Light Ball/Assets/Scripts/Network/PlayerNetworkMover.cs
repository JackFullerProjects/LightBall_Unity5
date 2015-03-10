using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Vector3 latestCorrectPos = Vector3.zero;
	private Quaternion correctPlayerRot;
	private Vector3 velocity;
	private float smoothing = 8f;
	bool initialLoad = true;
	

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
		if(PhotonNetwork.connected)
		{
			PhotonNetwork.sendRateOnSerialize = 5;
			PhotonNetwork.sendRate = 5;
			PhotonNetwork.isMessageQueueRunning = true;
		}

		PhotonNetwork.logLevel = PhotonLogLevel.Full;


		if(photonView.isMine)
		{
			GetComponent<FirstPersonController>().enabled = true;
            GetComponent<Player>().enabled = true;
            GetComponent<PlayerShoot>().enabled = true;
			GetComponent<Rigidbody>().useGravity = true;

			foreach(Transform child in transform)
			{
				if(child.transform.name == "FirstPersonCharacter")
				{
					child.GetComponent<Camera>().enabled = true;
					child.GetComponent<AudioListener>().enabled = true;
                    child.GetComponent<HeadBob>().enabled = true;

					foreach(Camera cam in child.GetComponentsInChildren<Camera>())
					{
						cam.enabled = true;
					}
				}
			}
		}
		else
		{
			latestCorrectPos = transform.position;
		}
	}

	void Update()
	{
        if(!photonView.isMine)
		    UpdateData();
	}
	
	private void UpdateData()
	{
		
		if(initialLoad)
		{
			initialLoad = false;
			transform.position = correctPlayerPos;
			transform.rotation = correctPlayerRot;
		}

        transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
        transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
	}




    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data
            correctPlayerPos = (Vector3)stream.ReceiveNext();
            correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }
}




//OLD IENUMATOR 
//	IEnumerator UpdateData()
//	{
//
//		while(true)
//		{
//			//only lerp position if it is needed
//			//UNUSED IN THIS VERSION
//			//if(Mathf.Abs((transform.position - correctPlayerPos).magnitude) > 0.001)
//			//{
//			//}
//			//===========================================================================
//
//
//			yield return null;
//		}
//	}










//	void Update()
//	{
//		if(!photonView.isMine)
//		{
//			SmoothMovement();
//		}
//	}
//
//	void SmoothMovement()
//	{
//		transform.localPosition =
//			Vector3.Lerp (transform.localPosition,
//			              latestCorrectPos, (float)(timestamp-prevtime));
//	}
//	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//	{
//		// Always send transform (depending on reliability of the network view)
//		if (stream.isWriting)
//		{
//			Vector3 pos = transform.localPosition;
//			stream.Serialize(ref pos);
//		}
//		// When receiving, buffer the information
//		else
//		{
//			// Receive latest state information
//			Vector3 pos = Vector3.zero;
//			stream.Serialize(ref pos);
//			
//			correctPlayerPos = latestCorrectPos;
//			latestCorrectPos = pos;
//			
//			prevtime = timestamp;
//			timestamp = info.timestamp;
//		}
//	}



//SMOOTHING V1




//	void Update()
//	{
//		if(photonView.isMine)
//			return;
//
//		SyncedMovement();
//	}
//
//
//	private void SyncedMovement()
//	{
//		syncTime += Time.deltaTime;
//		GetComponent<Rigidbody>().position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
//	}
//
//
//	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
//	{
//		Vector3 syncPosition = Vector3.zero;
//		if (stream.isWriting)
//		{
//			syncPosition = GetComponent<Rigidbody>().position;
//			stream.Serialize(ref syncPosition);
//		}
//		else
//		{
//			stream.Serialize(ref syncPosition);
//			
//			syncTime = 0f;
//			syncDelay = Time.time - lastSynchronizationTime;
//			lastSynchronizationTime = Time.time;
//			
//			syncStartPosition = GetComponent<Rigidbody>().position;
//			syncEndPosition = syncPosition;
//		}
//	}

