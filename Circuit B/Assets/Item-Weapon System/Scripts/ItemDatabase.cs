using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDatabase : EditorWindow
{
    private VisualElement _itemsTab;
    private ListView _itemListView;
    private float _itemHeight = 40;
    private VisualElement _detailItemSection;
    private VisualElement _scrollViewItemDetails;
    private VisualElement _largeDisplayIcon;

    private VisualElement _meleeStatsContainer;
    private VisualElement _gunStatsContainer;

    private SO_Item _activeItem;

    private Sprite _defaultItemIcon;
    private static VisualTreeAsset _itemRowTemplate;

    private static List<SO_Item> _itemDatabase = new List<SO_Item>();



    #region Save Vars
    Sprite newSprite;
    string newName;
    string newDescription;
    string newID;
    string newItemType;
    float newHitSpeed;
    float newDamage;
    float newReloadSpeed;
    int newMaxAmmoInclip;
    float newFireRange;
    float newFireSpeed;
    float newRecoilAmount;
    #endregion

    [MenuItem("Items/Item Database")]
    public static void Init()
    {
        ItemDatabase wnd = GetWindow<ItemDatabase>();
        wnd.titleContent = new GUIContent("Item Database");

        Vector2 size = new Vector2(900, 475);
        wnd.minSize = size;
        wnd.maxSize = size;
    }

    public void CreateGUI()
    {
        //Debug.Log("Create GUI");
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Item-Weapon System/Editor/UI/ItemDatabase.uxml");
        VisualElement rootFromUXML = visualTree.Instantiate();
        rootVisualElement.Add(rootFromUXML);

        _itemRowTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Item-Weapon System/Editor/UI/ItemRowTemplate.uxml");
        _defaultItemIcon = (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Item-Weapon System/Sprites/UnknownIcon.png", typeof(Sprite));

        LoadAllItems();

        _itemsTab = rootVisualElement.Q<VisualElement>("ItemsTab");

        GenerateItemListView();

        _detailItemSection = rootVisualElement.Q<VisualElement>("ItemContainer");
        _scrollViewItemDetails = rootVisualElement.Q<VisualElement>("ScrollViewItem_Details");
        _scrollViewItemDetails.style.display = DisplayStyle.None;

        _largeDisplayIcon = _detailItemSection.Q<VisualElement>("Icon");
        _meleeStatsContainer = _detailItemSection.Q<VisualElement>("MeleeStatsRow");
        _gunStatsContainer = _detailItemSection.Q<VisualElement>("GunStatsRow");

        ShowReliventData("");

        rootVisualElement.Q<Button>("btn_AddNewItem").clicked += AddNewItem_OnClick;
        rootVisualElement.Q<Button>("btn_AddNewMeleeWeapon").clicked += AddNewMeleeWeapon_OnClick;
        rootVisualElement.Q<Button>("btn_AddNewRangedWeapon").clicked += AddNewRangedWeapon_OnClick;

        #region Items

        #region General
        _detailItemSection.Q<TextField>("ItemName").RegisterValueChangedCallback(evt =>
        {
            newName = evt.newValue;
        });

        _detailItemSection.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
        {
            newDescription = evt.newValue;
        });

        _detailItemSection.Q<TextField>("ItemID").RegisterValueChangedCallback(evt =>
        {
            newID = evt.newValue;
        });

        _detailItemSection.Q<ObjectField>("IconPicker").RegisterValueChangedCallback(evt =>
        {
            newSprite = evt.newValue as Sprite;
            _largeDisplayIcon.style.backgroundImage = newSprite == null ? _defaultItemIcon.texture : newSprite.texture;
        });
        #endregion

        #region Melee
        _detailItemSection.Q<Slider>("sld_damage").RegisterValueChangedCallback(evt =>
        {
            newDamage = evt.newValue;
        });

        _detailItemSection.Q<Slider>("sld_hitSpeed").RegisterValueChangedCallback(evt =>
        {
            newHitSpeed = evt.newValue;
        });
        #endregion

        #region Gun
        _detailItemSection.Q<Slider>("sld_fireRange").RegisterValueChangedCallback(evt =>
        {
            newFireRange = evt.newValue;
        });

        _detailItemSection.Q<Slider>("sld_reloadSpeed").RegisterValueChangedCallback(evt =>
        {
            newReloadSpeed = evt.newValue;
        });

        _detailItemSection.Q<Slider>("sld_maxAmmoInclip").RegisterValueChangedCallback(evt =>
        {
            newMaxAmmoInclip = Mathf.RoundToInt(evt.newValue);
            _detailItemSection.Q<Slider>("sld_maxAmmoInclip").value = newMaxAmmoInclip;
        });

        _detailItemSection.Q<Slider>("sld_fireSpeed").RegisterValueChangedCallback(evt =>
        {
            newFireSpeed = evt.newValue;
        });

        _detailItemSection.Q<Slider>("sld_recoilAmount").RegisterValueChangedCallback(evt =>
        {
            newRecoilAmount = evt.newValue;
        });
        #endregion

        #endregion

        rootVisualElement.Q<Button>("btn_DeleteItem").clicked += DeleteItem_OnClick;
        rootVisualElement.Q<Button>("btn_SaveItem").clicked += SaveItem_OnClick;
    }

    private void LoadAllItems()
    {
        //Debug.Log("Loading Items");
        _itemDatabase.Clear();
        string[] allPaths = Directory.GetFiles("Assets/Item-Weapon System/Data", "*.asset", SearchOption.AllDirectories);
        foreach (string path in allPaths)
        {
            string cleanedPath = path.Replace("\\", "/");
            _itemDatabase.Add((SO_Item)AssetDatabase.LoadAssetAtPath(cleanedPath, typeof(SO_Item)));
        }
    }

    private void GenerateItemListView()
    {
        //Debug.Log("Generating Item List");
        Func<VisualElement> makeItem = () => _itemRowTemplate.CloneTree();

        Action<VisualElement, int> bindItem = (e, i) =>
        {
            e.Q<VisualElement>("Icon").style.backgroundImage = _itemDatabase[i] == null ? _defaultItemIcon.texture : _itemDatabase[i].itemSprite.texture;
            e.Q<Label>("Name").text = _itemDatabase[i].itemName;
        };
        _itemListView = new ListView(_itemDatabase, 35, makeItem, bindItem);
        _itemListView.selectionType = SelectionType.Single;
        _itemListView.style.height = _itemDatabase.Count * _itemHeight;
        _itemsTab.Add(_itemListView);
        _itemListView.selectionChanged += ItemListView_onSelectionChange;
    }

    private void ItemListView_onSelectionChange(IEnumerable<object> selectedItems)
    {
        _activeItem = (SO_Item)selectedItems.First();
        _scrollViewItemDetails.style.display = DisplayStyle.Flex;
        _scrollViewItemDetails.Q<TextField>("ItemName").value = _activeItem.itemName;
        _scrollViewItemDetails.Q<TextField>("Description").value = _activeItem.description;
        _scrollViewItemDetails.Q<TextField>("ItemID").value = _activeItem.itemId;
        _scrollViewItemDetails.Q<ObjectField>("IconPicker").value = _activeItem.itemSprite;

        if (_activeItem is SO_Weapon_Melee)
        {
            _scrollViewItemDetails.Q<Slider>("sld_damage").value = ((SO_Weapon_Melee)_activeItem).damage;
            _scrollViewItemDetails.Q<Slider>("sld_hitSpeed").value = ((SO_Weapon_Melee)_activeItem).hitSpeed;
        }

        if (_activeItem is SO_Weapon_Ranged)
        {
            _scrollViewItemDetails.Q<Slider>("sld_fireRange").value = ((SO_Weapon_Ranged)_activeItem).fireRange;
            _scrollViewItemDetails.Q<Slider>("sld_reloadSpeed").value = ((SO_Weapon_Ranged)_activeItem).reloadSpeed;
            _scrollViewItemDetails.Q<Slider>("sld_maxAmmoInclip").value = ((SO_Weapon_Ranged)_activeItem).maxAmmoInclip;
            _scrollViewItemDetails.Q<Slider>("sld_fireSpeed").value = ((SO_Weapon_Ranged)_activeItem).fireSpeed;
            _scrollViewItemDetails.Q<Slider>("sld_recoilAmount").value = ((SO_Weapon_Ranged)_activeItem).recoilAmount;
        }

        ShowReliventData(_activeItem.GetItemType());
        SerializedObject so = new SerializedObject(_activeItem);
        _detailItemSection.Bind(so);
        if (_activeItem.itemSprite != null)
        {
            _largeDisplayIcon.style.backgroundImage = _activeItem.itemSprite.texture;
        }
    }



    private void DeleteItem_OnClick()
    {
        //Get the path of the fie and delete it through AssetDatabase
        string path = AssetDatabase.GetAssetPath(_activeItem);
        AssetDatabase.DeleteAsset(path);

        //Purge the reference from the list and refresh the ListView
        _itemDatabase.Remove(_activeItem);
        _itemListView.Rebuild();

        //Nothing is selected, so hide the details section
        _scrollViewItemDetails.style.display = DisplayStyle.None;
    }

    private void SaveItem_OnClick()
    {
        string path = AssetDatabase.GetAssetPath(_activeItem);
        AssetDatabase.RenameAsset(path, newName);
        Sprite tempSprite = newSprite == null ? _defaultItemIcon : newSprite;
        _activeItem.itemSprite = tempSprite;
        _activeItem.description = newDescription;
        _activeItem.itemName = newName;
        _activeItem.itemId = newID;

        if (_activeItem is SO_Weapon_Melee)
        {
            ((SO_Weapon_Melee)_activeItem).damage = newDamage;
            ((SO_Weapon_Melee)_activeItem).hitSpeed = newHitSpeed;
        }

        if (_activeItem is SO_Weapon_Ranged)
        {
            ((SO_Weapon_Ranged)_activeItem).damage = newDamage;
            ((SO_Weapon_Ranged)_activeItem).fireRange = newFireRange;
            ((SO_Weapon_Ranged)_activeItem).fireSpeed = newFireSpeed;
            ((SO_Weapon_Ranged)_activeItem).recoilAmount = newRecoilAmount;
            ((SO_Weapon_Ranged)_activeItem).reloadSpeed = newReloadSpeed;
            ((SO_Weapon_Ranged)_activeItem).maxAmmoInclip = newMaxAmmoInclip;
        }
        EditorUtility.SetDirty(_activeItem);
        _itemListView.Rebuild();
        AssetDatabase.SaveAssets();
    }

    private void AddNewItem_OnClick()
    {
        int tempIndex = 0;
        //Create an instance of the scriptable object and set the default parameters
        SO_Item newItem = CreateInstance<SO_Item>();

        newItem.itemName = $"New Item";
        foreach (SO_Item item in _itemDatabase)
        {
            if (item.itemName == newItem.itemName)
            {
                tempIndex++;
            }
        }

        newItem.SetItemType("item");
        newItem.itemSprite = _defaultItemIcon;
        //Create the asset, using the unique ID for the name
        if (tempIndex == 0)
        {
            AssetDatabase.CreateAsset(newItem, $"Assets/Item-Weapon System/Data/Items/{newItem.itemName}.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(newItem, $"Assets/Item-Weapon System/Data/Items/{newItem.itemName}{tempIndex}.asset");
        }
        //Add it to the item list
        _itemDatabase.Add(newItem);
        //Refresh the ListView so everything is redrawn again
        _itemListView.Rebuild();
        _itemListView.style.height = _itemDatabase.Count * _itemHeight;
    }

    private void AddNewMeleeWeapon_OnClick()
    {
        int tempIndex = 0;
        //Create an instance of the scriptable object and set the default parameters
        SO_Weapon_Melee newItem = CreateInstance<SO_Weapon_Melee>();

        newItem.itemName = $"New MeleeWeapon Weapon";
        foreach (SO_Item item in _itemDatabase)
        {
            if (item.itemName == newItem.itemName)
            {
                tempIndex++;
            }
        }

        newItem.SetItemType("melee");
        newItem.itemSprite = _defaultItemIcon;
        //Create the asset, using the unique ID for the name
        if (tempIndex == 0)
        {
            AssetDatabase.CreateAsset(newItem, $"Assets/Item-Weapon System/Data/MeleeWeapon/{newItem.itemName}.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(newItem, $"Assets/Item-Weapon System/Data/MeleeWeapon/{newItem.itemName}{tempIndex}.asset");
        }
        //Add it to the item list
        _itemDatabase.Add(newItem);
        //Refresh the ListView so everything is redrawn again
        _itemListView.Rebuild();
        _itemListView.style.height = _itemDatabase.Count * _itemHeight;
    }

    private void AddNewRangedWeapon_OnClick()
    {
        int tempIndex = 0;
        //Create an instance of the scriptable object and set the default parameters
        SO_Weapon_Ranged newItem = CreateInstance<SO_Weapon_Ranged>();

        newItem.itemName = $"New Ranged Weapon";
        foreach (SO_Item item in _itemDatabase)
        {
            if (item.itemName == newItem.itemName)
            {
                tempIndex++;
            }
        }

        newItem.SetItemType("rangedWeapon");
        newItem.itemSprite = _defaultItemIcon;
        //Create the asset, using the unique ID for the name
        if (tempIndex == 0)
        {
            AssetDatabase.CreateAsset(newItem, $"Assets/Item-Weapon System/Data/Guns/{newItem.itemName}.asset");
        }
        else
        {
            AssetDatabase.CreateAsset(newItem, $"Assets/Item-Weapon System/Data/Guns/{newItem.itemName}{tempIndex}.asset");
        }
        //Add it to the item list
        _itemDatabase.Add(newItem);
        //Refresh the ListView so everything is redrawn again
        _itemListView.Rebuild();
        _itemListView.style.height = _itemDatabase.Count * _itemHeight;
    }

    private void ShowReliventData(String itemType)
    {
        if (itemType.Contains("melee", StringComparison.InvariantCultureIgnoreCase))
        {
            _meleeStatsContainer.style.display = DisplayStyle.Flex;
            _gunStatsContainer.style.display = DisplayStyle.None;
        }
        else if (itemType.Contains("rangedWeapon", StringComparison.InvariantCultureIgnoreCase))
        {
            _meleeStatsContainer.style.display = DisplayStyle.None;
            _gunStatsContainer.style.display = DisplayStyle.Flex;
        }
        else
        {
            _meleeStatsContainer.style.display = DisplayStyle.None;
            _gunStatsContainer.style.display = DisplayStyle.None;
        }
    }
}
