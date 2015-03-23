using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public Vector3 movementDir = Vector3.zero;
    private Rigidbody characterRigibody;
    public float speed;
    public bool isRight;

    public LayerMask raycastMask;
    // Use this for initialization
	void Start () {

        characterRigibody = GetComponent<Rigidbody>();
	
	}
	
	// Update is called once per frame
	void Update () {

        MovePlayer();
	
	}

    private void MovePlayer()
    {
        movementDir.z = -VirtualJoystick.movement.y * speed;
        movementDir.x = VirtualJoystick.movement.x * (speed / 2);

        if (movementDir.x > 0)
            isRight = true;
        else
            isRight = false;

        characterRigibody.velocity = movementDir;
    }


    public void Attack()
    {

        RaycastHit hit;
        float raycastDistance = 2f;

        if (isRight)
        {
            Vector3 raycastDirection = new Vector3(1f, 1f, 0f);

            //FORWARD RAYCASTS
            //=================================================================================
            var raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);

            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);
            if (hit.collider != null)
                AttackNPC(hit.collider.gameObject);

            if (!raycastHit)
            {
                raycastHit = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right) * raycastDistance, out hit, raycastMask);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * raycastDistance, Color.red);

                if (hit.collider != null)
                    AttackNPC(hit.collider.gameObject);

                if (!raycastHit)
                {
                    raycastDirection = new Vector3(1f, -1f, 0f);
                    raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                    Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                    if (hit.collider != null)
                        AttackNPC(hit.collider.gameObject);
                }
            }

            //RIGHT FORWARD RAYCASTS
            //=================================================================================
            raycastDirection = new Vector3(1f, 1f, 1f);
            raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

            if (hit.collider != null)
                AttackNPC(hit.collider.gameObject);

            if (!raycastHit)
            {
                raycastDirection = new Vector3(1f, 0f, 1f);
                raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                if (hit.collider != null)
                    AttackNPC(hit.collider.gameObject);

                if (!raycastHit)
                {
                    raycastDirection = new Vector3(1f, -1f, 1f);
                    raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                    Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                    if (hit.collider != null)
                        AttackNPC(hit.collider.gameObject);
                }
            }

            //LEFT FORWARD RAYCATS
            //===================================================================================
            raycastDirection = new Vector3(1f, 1f, -1f);
            raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

            if (hit.collider != null)
                AttackNPC(hit.collider.gameObject);

            if (!raycastHit)
            {
                raycastDirection = new Vector3(1f, 0f, -1f);
                raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                if (hit.collider != null)
                    AttackNPC(hit.collider.gameObject);

                if (!raycastHit)
                {
                    raycastDirection = new Vector3(1f, -1f, -1f);
                    raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                    Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                    if (hit.collider != null)
                        AttackNPC(hit.collider.gameObject);
                }
            }
            //=====================================================================================

        }
        else
        {
            Vector3 raycastDirection = new Vector3(-1f, 1f, 0f);

            //FORWARD RAYCASTS
            //=================================================================================
            var raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

            if (hit.collider != null)
                AttackNPC(hit.collider.gameObject);

            if (!raycastHit)
            {
                raycastHit = Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.right) * raycastDistance, out hit, raycastMask);
                Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.right) * raycastDistance, Color.red);

                if (hit.collider != null)
                    AttackNPC(hit.collider.gameObject);

                if (!raycastHit)
                {
                    raycastDirection = new Vector3(-1f, -1f, 0f);
                    raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                    Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                    if (hit.collider != null)
                        AttackNPC(hit.collider.gameObject);
                }
            }

            //RIGHT FORWARD RAYCASTS
            //=================================================================================
            raycastDirection = new Vector3(-1f, 1f, 1f);
            raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

            if (hit.collider != null)
                AttackNPC(hit.collider.gameObject);

            if (!raycastHit)
            {
                raycastDirection = new Vector3(-1f, 0f, 1f);
                raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                if (hit.collider != null)
                    AttackNPC(hit.collider.gameObject);

                if (!raycastHit)
                {
                    raycastDirection = new Vector3(-1f, -1f, 1f);
                    raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                    Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                    if (hit.collider != null)
                        AttackNPC(hit.collider.gameObject);

                }
            }

            //LEFT FORWARD RAYCATS
            //===================================================================================
            raycastDirection = new Vector3(-1f, 1f, -1f);
            raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

            if (hit.collider != null)
                AttackNPC(hit.collider.gameObject);

            if (!raycastHit)
            {
                raycastDirection = new Vector3(-1f, 0f, -1f);
                raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                if (hit.collider != null)
                    AttackNPC(hit.collider.gameObject);

                if (!raycastHit)
                {
                    raycastDirection = new Vector3(-1f, -1f, -1f);
                    raycastHit = Physics.Raycast(transform.position, raycastDirection * raycastDistance, out hit, raycastMask);
                    Debug.DrawRay(transform.position, raycastDirection * raycastDistance, Color.red);

                    if (hit.collider != null)
                        AttackNPC(hit.collider.gameObject);

                }
            }
            //=====================================================================================
        }
    }

    private void AttackNPC(GameObject _NPC)
    {
        if (_NPC.tag == "NPC")
        {
            var npcHealth = _NPC.gameObject.GetComponent<NPChealth>();

            npcHealth.curHealth -= 10;
            return;
        }
    }

    void OnGUI()
    {
        //GUI.Box(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 100, 100, 100), "" + isRight);
    }
}
