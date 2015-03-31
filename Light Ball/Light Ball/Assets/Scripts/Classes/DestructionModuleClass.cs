using UnityEngine;
using System.Collections;

[System.Serializable]
public class DestructionModuleClass
{
    #region Class Variables
    public Mesh Model;
    public Animation[] GunAnimations;
    public int Range;
    public float Accuracy;
    private int ammo;
    public int Ammo
    {
        get
        {
            if (ammo > MaxAmmo)
                ammo = MaxAmmo;

            return ammo;
        }
        set
        {
             ammo = value;
        }
    }

    [HideInInspector]
    public int ShotsInClip;
    public int MaxAmmo;
    public float ReloadTime;
    public int ClipSize;
    public int HealthDamage;
    public int ForceFieldDamage;
    public float ModuleCooldown;

    public bool IsAOE;
    public float AOESize;
    public int AOEDamage;
    #endregion

    #region Class Constructors
    public DestructionModuleClass(int _range, float _accuracy, float _cooldown, int _healthDamage, int _ammo, int _forceFieldDamage)
    {
        Range = _range;
        Accuracy = _accuracy;
        ModuleCooldown = _cooldown;
        Ammo += _ammo;
        HealthDamage = _healthDamage;
        ForceFieldDamage = _forceFieldDamage;
    }
    #endregion

    #region Class Methods e.g Reload/Play Animation
    public void Reload()
    {
        if (Ammo > 0)//if we have ammo
        {
           // Debug.Log("ALL AMMO: " + Ammo);
            if (ShotsInClip < ClipSize)// if we have used a shot in the clip
            {
                int ammoToReload = ClipSize - ShotsInClip; // calculate amount of ammo to reload
                // Debug.Log("AMMO NEEDED TO RELOAD: " + ammoToReload);
                int checkAmmoWeHave = Ammo - ammoToReload;
                //Debug.Log("AMMO WE HAVE AFTER RELOAD " + checkAmmoWeHave);
                if (checkAmmoWeHave > 0)
                {
                    //RELOAD
                    Ammo -= ammoToReload;
                    ShotsInClip = ammoToReload;
                }
                else
                {
                    //if we didnt have enough the calculate the number we are short then reload whats left
                    ammoToReload = ClipSize - Mathf.Abs(checkAmmoWeHave);
                    Ammo -= ammoToReload;
                    ShotsInClip += ammoToReload;

                }
            }
        }
    }

    public void PlayFireAnimation()
    {
    }

    public void PlayReloadAnimation()
    {

    }
    #endregion
}
