using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon : InteractableItem
{
    
    public override void Pickup()
    {
        Debug.Log($"Picked Up");
    }

    public override void Use()
    {
        Debug.Log($"Use Weapon");
    }
}
