using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    private RigidbodyFirstPersonController RPNM_Script;

    private bool PlayerShooting = false;

    //shooting cooldown
    public float shootCooldown;
    private float storeShootCooldown;
    private bool canFire = true;

    void Start()
    {
        storeShootCooldown = shootCooldown;
        RPNM_Script = gameObject.GetComponent<RigidbodyFirstPersonController>();
    }
	
	// Update is called once per frame
	void Update () {

        if (canFire)
        {
            if (RPNM_Script.ControllerInUse)
            {
                if (Input.GetAxis("R_Trigger_1") > 0 && !PlayerShooting)
                {
                    canFire = false;
                    var player = GetComponent<Player>();
                    player.Fire();
                    PlayerShooting = true;
                    StartCoroutine(ShootingCooldown(0.2f));
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    canFire = false;
                    var player = GetComponent<Player>();
                    player.Fire();
                }
            }
        }
        else
        {
            shootCooldown -= Time.deltaTime;
            if(shootCooldown <= 0)
            {
                canFire = true;
                shootCooldown = storeShootCooldown;
            }
        }
	
	}

    IEnumerator ShootingCooldown(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PlayerShooting = false;
    }
}
