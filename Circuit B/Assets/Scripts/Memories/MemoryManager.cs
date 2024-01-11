using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManager : MonoBehaviour, IDataPersistance
{
    [SerializeField] List<Memories> _memoriesDefault = new List<Memories>();
    List<Memories> _memoriesInGame = new List<Memories>();
    [SerializeField] GameObject _content;
    [SerializeField] GameObject _buttonPrefab;
    [SerializeField] GameObject _objectPrefab;
    [SerializeField] TextMeshProUGUI _memoryTitle;
    [SerializeField] TextMeshProUGUI _memoryDescription;
    [SerializeField] Image _memoryImage;

    public static MemoryManager Instance { get; private set; }
    public List<Memories> Memories { get { return _memoriesDefault; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _memoriesInGame = DeepCopyMemoriesList(_memoriesDefault);
    }

    private void Start()
    {
        foreach (Memories memory in _memoriesInGame)
        {
            memory.MemoryButton = Instantiate(_buttonPrefab, _content.transform);
            memory.MemoryButton.GetComponent<MemoryButton>().ButtonText.text = memory.MemoryName;
            memory.MemoryButton.GetComponent<MemoryButton>().Memory = memory;
        }
    }

    public void UnlockMemory(string memoryToFind)
    {
        Memories tempMemory = _memoriesInGame.Find(r => r.MemoryName == memoryToFind);
        tempMemory.HasCollected = true;
        tempMemory.MemoryButton.GetComponent<MemoryButton>().ButtonText.text = tempMemory.MemoryName;
        UpdateMemoryViewer(memoryToFind);
    }

    public void UpdateMemoryViewer(string memoryToFind, bool lockMemory = false)
    {
        Memories temp = _memoriesInGame.Find(r => r.MemoryName == memoryToFind);
        if (!lockMemory)
        {
            _memoryTitle.text = temp.SOMemory.Title;
            _memoryDescription.text = temp.SOMemory.Description;
            _memoryImage.sprite = temp.SOMemory.Picture;
        }
        else
        {
            _memoryTitle.text = "";
            _memoryDescription.text = "";
            _memoryImage.sprite = null;
        }
    }

    public void LockMemory(string memoryToFind)
    {
        Memories tempMemory = _memoriesInGame.Find(r => r.MemoryName == memoryToFind);
        tempMemory.HasCollected = false;
        tempMemory.MemoryButton.GetComponent<MemoryButton>().ButtonText.text = "???";
        UpdateMemoryViewer(memoryToFind, true);
    }

    public void LoadData(GameData gameData)
    {
        foreach (Memories memory in _memoriesInGame)
        {
            Destroy(memory.MemoryObject);
            memory.HasCollected = false;
            if (memory.MemoryButton != null)
            {
                LockMemory(memory.MemoryName);
            }
        }

        foreach (Memories mem in gameData.memories)
        {
            //Debug.Log($"{gameData.uuid} - {mem.MemoryName}, {mem.HasCollected}");
            if (mem.HasCollected)
            {
                UnlockMemory(mem.MemoryName);
                //mem.MemoryObject.gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log($"{mem.MemoryName}, Object:{mem.MemoryObject} Spawned at ${mem.SpawnLocation}");
                Memories tempMemory = _memoriesInGame.Find(r => r.MemoryName == mem.MemoryName);
                tempMemory.MemoryObject = Instantiate(_objectPrefab, mem.SpawnLocation, Quaternion.identity);
                tempMemory.MemoryObject.GetComponent<MemoryObject>().MemoryName = mem.MemoryName;
                //mem.MemoryObject.gameObject.SetActive(true);
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.memories = _memoriesInGame;
    }

    private List<Memories> DeepCopyMemoriesList(List<Memories> sourceList)
    {
        return sourceList.Select(memory => new Memories
        (
            memory.MemoryButton,
            memory.HasCollected,
            memory.SpawnLocation,
            memory.SOMemory
        )).ToList();
    }
}

[System.Serializable]
public class Memories
{
    //[SerializeField] string _memoryName;
    [HideInInspector] GameObject _memoryObject;
    [HideInInspector] GameObject _memoryButton;
    [SerializeField] bool _hasCollected = false;
    [SerializeField] Vector3 _spawnLocation;
    [SerializeField] SO_Memory _soMemory;

    public string MemoryName { get { return _soMemory.Title; } }
    public GameObject MemoryObject { get { return _memoryObject; } set { _memoryObject = value; } }
    public GameObject MemoryButton { get { return _memoryButton; } set { _memoryButton = value; } }
    public bool HasCollected { get { return _hasCollected; } set { _hasCollected = value; } }
    public Vector3 SpawnLocation { get { return _spawnLocation; } set { _spawnLocation = value; } }
    public SO_Memory SOMemory { get { return _soMemory; } }

    public Memories(GameObject memoryButton, bool hasCollected, Vector3 spawnLocation, SO_Memory soMemory)
    {
        //_memoryObject = memoryObject;
        _memoryButton = memoryButton;
        _hasCollected = hasCollected;
        _spawnLocation = spawnLocation;
        _soMemory = soMemory;
    }
}
