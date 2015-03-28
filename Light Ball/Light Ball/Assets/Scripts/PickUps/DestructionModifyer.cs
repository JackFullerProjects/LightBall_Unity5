using UnityEngine;
using System.Collections;

public class DestructionModifyer : Photon.MonoBehaviour {

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

    [Header("Damage Variables")]
    public int HealthDamage;
    public int ArmourDamage;
    public int ForceFieldDamage;



    void OnCollisionEnter(Collision other)
    {
        var EditDestructionModule = (IEditAble) other.collider.gameObject.GetComponent(typeof(IEditAble));

        if (EditDestructionModule == null)
            return;

        EditDestructionModule.DestructionModify(Ammo, Cooldown, Accuracy, Range, ArmourDamage, HealthDamage, ForceFieldDamage);
    }
}
