﻿using UnityEngine;
using System.Collections;

public interface IEditAble
{
    void DestructionModify(int ammo, float cooldown, float accuracy, int range, int armourdamage, int healthdamage, int forcefieldDamage);

    void ImpairmentModify();
}
