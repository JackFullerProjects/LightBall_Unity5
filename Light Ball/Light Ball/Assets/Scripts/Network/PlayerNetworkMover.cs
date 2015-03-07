using UnityEngine;
using System.Collections;

public class PlayerNetworkMover : Photon.MonoBehaviour {

	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;


	// Use this for initialization
	void Start ()
	{
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
		while(true)
		{
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			yield return null;
		}
	}

	void OnPhotonSerializaView(PhotonStream _stream, PhotonMessageInfo _info)
	{
		if(_stream.isWriting)
		{
			_stream.SendNext(transform.position);
			_stream.SendNext(transform.rotation);
		}
		else
		{
			position = (Vector3)_stream.ReceiveNext();
			rotation = (Quaternion)_stream.ReceiveNext();
		}
	}
}

