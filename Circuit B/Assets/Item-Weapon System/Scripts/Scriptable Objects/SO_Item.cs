using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Data/Item Data")]
public class SO_Item : ScriptableObject
{
    [SerializeReference] public string itemName;
    [SerializeReference] public Sprite itemSprite;
    [SerializeReference] public string description;
    [SerializeReference] public string itemId;
    [SerializeReference] private string _itemType;

    public void SetItemType(string newItemType)
    {
        _itemType = newItemType;
    }

    public string GetItemType()
    {
        return _itemType;
    }
}
