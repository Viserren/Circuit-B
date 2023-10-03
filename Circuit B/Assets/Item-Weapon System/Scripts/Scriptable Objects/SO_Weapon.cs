using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/Weapon Data")]
public class SO_Weapon : SO_Item
{
    [SerializeReference] public float damage;
}
