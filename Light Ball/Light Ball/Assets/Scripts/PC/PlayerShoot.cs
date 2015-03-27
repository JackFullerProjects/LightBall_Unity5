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
            if (RigidbodyFirstPersonController.UsingController)
            {
                if (Input.GetAxis("R_Trigger_1") > 0 && !PlayerShooting)
                {
                    if (!GetComponent<Player>().GunAnimation.GetComponent<Animation>().isPlaying)
                    {
                        canFire = false;
                        var player = GetComponent<Player>();
                        player.Fire();
                        PlayerShooting = true;
                        //StartCoroutine(ShootingCooldown(0.2f));
                        StartCoroutine(FireCooldown(shootCooldown));
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && canFire)
                {
                    if (!GetComponent<Player>().GunAnimation.GetComponent<Animation>().isPlaying)
                    {
                        canFire = false;
                        var player = GetComponent<Player>();
                        player.Fire();
                        StartCoroutine(FireCooldown(shootCooldown));
                    }
                }
            }
        }
	
	}

    IEnumerator FireCooldown(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        canFire = true;
        PlayerShooting = false;
    }

    IEnumerator ShootingCooldown(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        PlayerShooting = false;
    }
}
