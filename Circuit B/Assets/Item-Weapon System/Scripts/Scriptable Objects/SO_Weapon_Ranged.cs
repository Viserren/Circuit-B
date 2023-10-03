using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon Data", menuName = "Data/Ranged Weapon Data")]
public class SO_Weapon_Ranged : SO_Weapon
{
    [SerializeReference] public float reloadSpeed;
    [SerializeReference] public int maxAmmoInclip;
    [SerializeReference] public float fireRange;
    [SerializeReference] public float fireSpeed;
    [SerializeReference] public float recoilAmount;
}
