using UnityEngine;
using System.Collections;

[System.Serializable]
public class ImpairmentModuleClass
{
    public int Ammo;
    public float Range;
    public float ModuleCooldown;

    public ImpairmentModuleClass(int _ammo, float _range)
    {
        Ammo = _ammo;
        Range = _range;
    }
}
