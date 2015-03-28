using UnityEngine;
using System.Collections;

[System.Serializable]
public class DestructionModuleClass
{
    public int Range;
    public float Accuracy;
    public int Ammo;
    public int ArmourDamage;
    public int HealthDamage;
    public int ForceFieldDamage;
    public float ModuleCooldown;

    public bool IsAOE;
    public float AOESize;
    public int AOEDamage;

    public DestructionModuleClass(int _range, float _accuracy, float _cooldown, int _armourDamage, int _healthDamage, int _ammo, int _forceFieldDamage)
    {
        Range = _range;
        Accuracy = _accuracy;
        ModuleCooldown = _cooldown;
        Ammo += _ammo;
        ArmourDamage = _armourDamage;
        HealthDamage = _healthDamage;
        ForceFieldDamage = _forceFieldDamage;
    }
}
