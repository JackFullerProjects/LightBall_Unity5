using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    private RigidbodyFirstPersonController RPNM_Script;

    private bool PlayerShooting = false;

    void Start()
    {
        RPNM_Script = gameObject.GetComponent<RigidbodyFirstPersonController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (RPNM_Script.ControllerInUse)
        {
            if (Input.GetAxis("R_Trigger_1") > 0 && !PlayerShooting)
            {
                var player = GetComponent<Player>();
                player.Fire();
                PlayerShooting = true;
                StartCoroutine(ShootingCooldown());
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                var player = GetComponent<Player>();
                player.Fire();
            }
        }     
	
	}

    IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(0.2F);
        PlayerShooting = false;
    }
}
