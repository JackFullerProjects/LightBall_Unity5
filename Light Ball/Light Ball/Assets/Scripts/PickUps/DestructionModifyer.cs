using UnityEngine;
using System.Collections;

public class DestructionModifyer : Photon.MonoBehaviour {

    [Header("3D Mesh for the Gun")]
    public Mesh Gun;
    [Header("Loads Animations for Gun")]
    public Animation[] GunAnimations;
    [Header("Amount of Ammo in each Clip of the Gun")]
    public int ClipSize;
    [Header("Maximum Amount of Ammo tha player can hold with this gun")]
    public int MaxAmmo;
    [Header("Controls the ammo to add to the player who picks up object")]
    public int Ammo;
    [Header("Controls the cooldown of the gun")]
    public float Cooldown;
    [Header("Controls accuracy 0 being 100%")]
    [Range(0f, 0.2f)]
    public float Accuracy;
    [Header("Controls the range of the weapons raycast")]
    [Range(0, 20000)]
    public int Range;
    [Header("Reload Time In Seconds")]
    [Range(0, 10)]
    public float ReloadTime;

    [Header("Damage Variables")]
    public int HealthDamage;
    public int ForceFieldDamage;

    [Header("AOE Variables")]
    [Header("Gives the Gun an AOE")]
    public bool IsAOE;
    [Header("Size of the AOE Area")]
    public float AOESize;
    [Header("Damage done to each enemy in the AOE")]
    [Range(0, 100)]
    public int AOEDamage;



    void OnCollisionEnter(Collision other)
    {
        var EditDestructionModule = (IEditAble) other.collider.gameObject.GetComponent(typeof(IEditAble));

        if (EditDestructionModule == null)
            return;

        EditDestructionModule.DestructionModify(Ammo, ClipSize, MaxAmmo, ReloadTime, Cooldown, Accuracy, Range, HealthDamage, ForceFieldDamage);
    }
}
