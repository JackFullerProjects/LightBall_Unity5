using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private float smoothing = 5f;
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
		else 
		{
			StartCoroutine("UpdateData");
		}
	}
	

	IEnumerator UpdateData()
	{
		if(initialLoad)
		{
			initialLoad = false;
			transform.position = correctPlayerPos;
			transform.rotation = correctPlayerRot;
		}
		while(true)
		{
			//only lerp position if it is needed
			//UNUSED IN THIS VERSION
			//if(Mathf.Abs((transform.position - correctPlayerPos).magnitude) > 0.001)
			//{
			//}
			//===========================================================================

			this.transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * smoothing);
			this.transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * smoothing);

			yield return null;
		}
	}

	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		Debug.Log("calling");
		if (stream.isWriting)
		{
			Debug.Log("writing");
			stream.SendNext(this.transform.position);
			stream.SendNext(this.transform.rotation);
			
		}
		else
		{
			Debug.Log("reading");
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}

}

