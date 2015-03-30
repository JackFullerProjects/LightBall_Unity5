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
    Player player;

    void Start()
    {
        storeShootCooldown = destructionCooldown;
        RPNM_Script = gameObject.GetComponent<RigidbodyFirstPersonController>();

        ImpairmentCooldown = GetComponent<Player>().impairmentModuleClass.ModuleCooldown;
        destructionCooldown = GetComponent<Player>().destructionModuleClass.ModuleCooldown;
        player = GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update () {

        if (canFire)
        {
            if (RigidbodyFirstPersonController.UsingController)
            {
                if (Input.GetAxis("R_Trigger_1") > 0 && !PlayerShooting)
                {
                    if (!player.GunAnimation.GetComponent<Animation>().isPlaying)
                    {
                        if (player.ballIndex == 0)
                        {
                            if (player.destructionModuleClass.ShotsInClip > 0)
                            {
                                player.destructionModuleClass.ShotsInClip -= 1;
                                canFire = false;
                                player.Fire();
                                PlayerShooting = true;
                                StartCoroutine(FireCooldown(destructionCooldown));
                            }
                            else
                            {
                                if(player.destructionModuleClass.Ammo > 0)
                                     player.destructionModuleClass.Reload();
                            }
                        }
                        else
                        {
                            canFire = false;
                            player.Fire();
                            PlayerShooting = true;
                            StartCoroutine(FireCooldown(ImpairmentCooldown));
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && canFire)
                {
                    if (!player.GunAnimation.GetComponent<Animation>().isPlaying)
                    {
                        if (player.ballIndex == 0)
                        {
                            if (player.destructionModuleClass.ShotsInClip > 0)
                            {
                                player.destructionModuleClass.ShotsInClip -= 1;
                                canFire = false;
                                player.Fire();
                                PlayerShooting = true;
                                StartCoroutine(FireCooldown(destructionCooldown));
                            }
                            else
                            {
                                if (player.destructionModuleClass.Ammo > 0)
                                    player.destructionModuleClass.Reload();
                            }
                        }
                        else
                        {
                            canFire = false;
                            player.Fire();
                            PlayerShooting = true;
                            StartCoroutine(FireCooldown(ImpairmentCooldown));
                        }
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
