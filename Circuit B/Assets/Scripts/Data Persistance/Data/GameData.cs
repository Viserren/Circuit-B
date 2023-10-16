using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int deathCount;
    public List<InventoryItem> inventory;

    /* 
     * The values defined in this constructor will be the default values
     * when the game is first played and no data has been saved yet.
    */ 
    public GameData()
    {
        this.deathCount = 0;
        this.inventory = new List<InventoryItem>();
    }
}
