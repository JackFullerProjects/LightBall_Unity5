using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    private RigidbodyFirstPersonController RPNM_Script;

    private bool PlayerShooting = false;

    //shooting cooldown
    [HideInInspector]
    public float destructionCooldown;
    [HideInInspector]
    public float ImpairmentCooldown;
    private float storeShootCooldown;
    private bool canFire = true;

    void Start()
    {
        storeShootCooldown = destructionCooldown;
        RPNM_Script = gameObject.GetComponent<RigidbodyFirstPersonController>();

        ImpairmentCooldown = GetComponent<Player>().impairmentModuleClass.ModuleCooldown;
        destructionCooldown = GetComponent<Player>().destructionModuleClass.ModuleCooldown;
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
                        if (GetComponent<Player>().ballIndex == 0)
                            StartCoroutine(FireCooldown(destructionCooldown));
                        else
                            StartCoroutine(FireCooldown(ImpairmentCooldown));
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

                        if(GetComponent<Player>().ballIndex == 0)
                             StartCoroutine(FireCooldown(destructionCooldown));
                        else
                            StartCoroutine(FireCooldown(ImpairmentCooldown));
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
