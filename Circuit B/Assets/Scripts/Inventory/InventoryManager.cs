//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//public class InventoryManager : MonoBehaviour, IDataPersistance
//{
//    public SO_Item encryped;
//    private List<InventoryItem> items = new List<InventoryItem>();

//    public void AddItemToIntentory(SO_Item itemToAdd, int amountToAdd = 1)
//    {
//        try
//        {
//            InventoryItem currentItem = items.Single(item => item.item == itemToAdd);
//            currentItem.IncreaseAmount(amountToAdd);
//            Debug.Log($"{currentItem.name}, x{currentItem.quantity}");
//        }
//        catch (System.InvalidOperationException)
//        {
//            items.Add(new InventoryItem(itemToAdd, amountToAdd));
//            Debug.Log($"New Item Found: {itemToAdd.name}");
//        }
//    }

//    public void RemoveItemFromIntentory(SO_Item itemToRemove, int amountToRemove = 1)
//    {
//        try
//        {
//            InventoryItem currentItem = items.Single(item => item.item == itemToRemove);
//            currentItem.DecreaseAmount(amountToRemove);
//            Debug.Log($"{currentItem.name}, x{currentItem.quantity}");
//        }
//        catch (System.InvalidOperationException)
//        {
//            Debug.LogError("How you removing a item that isnt there???");
//        }
//    }

//    public void ClearInventory()
//    {
//        items.Clear();
//    }

//    public void LoadData(GameData gameData)
//    {
//        this.items = gameData.inventory;
//    }

//    public void SaveData(ref GameData gameData)
//    {
//        gameData.inventory = this.items;
//    }
//}

//[System.Serializable]
//public class InventoryItem
//{
//    public string name { get; private set; }
//    public SO_Item item { get; private set; }
//    public int quantity { get; private set; }

//    public InventoryItem(SO_Item item, int quantity)
//    {
//        name = item.name;
//        this.item = item;
//        this.quantity = quantity;
//    }

//    public void IncreaseAmount(int amountToAdd)
//    {
//        quantity += amountToAdd;
//    }

//    public void DecreaseAmount(int amountToRemove)
//    {
//        quantity -= amountToRemove;
//        if (quantity <= 0)
//        {
//            quantity = 0;
//        }
//    }
//}

//[CustomEditor(typeof(InventoryManager))]
//public class InventoryManagerEditor : Editor
//{
//    SerializedProperty inventoryItems;
//    private void OnEnable()
//    {
//        inventoryItems = serializedObject.FindProperty("items");
//    }

//    public override void OnInspectorGUI()
//    {
//        InventoryManager inventoryManager = (InventoryManager)target;
//        base.OnInspectorGUI();
//        if (GUILayout.Button("Add Item"))
//        {
//            inventoryManager.AddItemToIntentory(inventoryManager.encryped);
//        }

//        if (GUILayout.Button("Remove Item"))
//        {
//            inventoryManager.RemoveItemFromIntentory(inventoryManager.encryped);
//        }

//        if (GUILayout.Button("Clear List"))
//        {
//            inventoryManager.ClearInventory();
//        }
//    }
//}
