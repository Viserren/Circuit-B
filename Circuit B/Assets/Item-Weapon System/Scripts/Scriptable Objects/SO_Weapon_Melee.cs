using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon Weapon Data", menuName = "Data/MeleeWeapon Weapon Data")]
public class SO_Weapon_Melee : SO_Weapon
{
    [SerializeReference] public float hitSpeed;
}
