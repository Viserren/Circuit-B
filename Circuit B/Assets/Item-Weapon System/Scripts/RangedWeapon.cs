using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedWeapon : Weapon, Reloadable
{
    public SO_Weapon_Ranged rangedWeapon;

    public void Reload()
    {
        Debug.Log($"Weapon: {rangedWeapon.itemName} reloading");
    }

    public override void Use()
    {
        Debug.Log($"Current Ranged Weapon: {rangedWeapon.itemName}, damage: {rangedWeapon.damage}");
    }

    public override void Pickup()
    {
        base.Pickup();
    }
}
