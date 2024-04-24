using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string uuid;
    public string saveName;
    public int deathCount;
    public int health;
    public int maxHealth;
    public Vector3 startLocation;
    public string currentLocation;
    public Quaternion startRotation;
    //public List<InventoryItem> inventory;
    public string dateLastSaved;
    public string dateCreated;
    public bool isDead;
    public bool hasBattery;

    // Memories
    public List<Memories> memories;


    /* 
     * The values defined in this constructor will be the default values
     * when the game is first played and no data has been saved yet.
    */
    public GameData(string uuid, string saveName)
    {
        this.uuid = uuid;
        this.saveName = saveName;
        this.dateLastSaved = new System.DateTime().ToString();
        this.dateCreated = System.DateTime.Now.ToString();
        this.currentLocation = "House Area";
        this.deathCount = 0;
        this.health = 10;
        this.maxHealth = 10;
        this.isDead = false;
        this.hasBattery = false;
        //this.inventory = new List<InventoryItem>();
        this.startLocation = new Vector3(31.9375f, -4.09375f, 53.03125f);
        this.startRotation = new Quaternion(0, -0.216590568f, 0, -0.976262569f);
        this.memories = MemoryManager.Instance.Memories;
    }
}
