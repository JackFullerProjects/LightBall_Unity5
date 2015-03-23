using UnityEngine;
using System.Collections;

public class NPCmovement : MonoBehaviour {

    public GameObject PC;
    public float speed;
    public Vector3 movementDir;
    private Rigidbody npcRigidbody;
	// Use this for initialization
	void Start () 
    {
        npcRigidbody = GetComponent<Rigidbody>();
        PC = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
         Move();
    }

    private void Move()
    {
        movementDir = PC.transform.position - transform.position;
        npcRigidbody.velocity = movementDir * speed;
    }
}
