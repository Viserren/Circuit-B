using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MeleeWeapon : Weapon
{
    public SO_Weapon_Melee melee;

    public override void Use()
    {
        Debug.Log($"Current Melee Weapon: {melee.itemName}, damage: {melee.damage}");
    }
}
