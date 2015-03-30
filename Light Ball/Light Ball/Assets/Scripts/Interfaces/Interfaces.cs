using UnityEngine;
using System.Collections;

public interface IEditAble
{
    void DestructionModify(int ammo, int clipSize, int MaxAmmo, float reloadTime, float cooldown, float accuracy, int range, int healthdamage, int forcefieldDamage);

    void ImpairmentModify();
}
