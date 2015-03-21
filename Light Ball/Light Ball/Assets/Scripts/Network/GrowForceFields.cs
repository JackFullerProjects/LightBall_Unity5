using UnityEngine;
using System.Collections;

public class GrowForceFields : Photon.MonoBehaviour {

    Vector3 correctScale;

    public float lerpTime = 1f;
    float currentLerpTime;
    Vector3 startScale = new Vector3(0.1f,0.1f,0.1f);
    Vector3 endScale = new Vector3(1f,1f,1f);

    void Start()
    {
        transform.localScale = startScale;
    }
	void Update () 
    {
        Scale();
	}

    private void Scale()
    {
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }

        float perc = currentLerpTime / lerpTime;
        transform.localScale = Vector3.Lerp(startScale, endScale, perc);
    }

}
