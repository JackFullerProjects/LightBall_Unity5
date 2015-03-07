using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private float smoothing = 10f;
	bool initialLoad = true;


	// Use this for initialization
	void Start ()
	{
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		if(photonView.isMine)
		{
			GetComponent<FirstPersonController>().enabled = true;
			GetComponent<AudioSource>().enabled = true;
			GetComponent<Rigidbody>().useGravity = true;

			foreach(Transform child in transform)
			{
				if(child.transform.name == "FirstPersonCharacter")
				{
					child.GetComponent<Camera>().enabled = true;
					child.GetComponent<AudioListener>().enabled = true;

					foreach(Camera cam in child.GetComponentsInChildren<Camera>())
					{
						cam.enabled = true;
					}
				}
			}

		}
	}

	// Update is called once per frame
	void Update () {

		if (photonView.isMine) {
			//Do nothing
		}
		else {
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * smoothing);
		}
	}



	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log("calling");
		if (stream.isWriting)
		{
			Debug.Log("writing");
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			
		}
		else
		{
			Debug.Log("reading");
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

}

