using UnityEngine;
using System.Collections;

public class DestroyObject : MonoBehaviour {

    public float DestroyTime;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}
