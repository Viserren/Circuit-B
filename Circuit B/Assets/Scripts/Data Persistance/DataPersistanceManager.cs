using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Settings")]
    [SerializeField] private string _fileName;


    private GameData _gameData;
    private List<IDataPersistance> _dataPersistanceObjects;

    private FileDataHandler _fileDataHandler;

    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple instances of DataPersistanceManager");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, this._fileName);
        this._dataPersistanceObjects = FindAllDataPersistanceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        // Load any saved data from a file using the data handler
        this._gameData = _fileDataHandler.Load();

        // If no data was found, initalize the game with defaults
        if(this._gameData == null)
        {
            Debug.Log("No data was found. Initalizing game with defaults.");
            NewGame();
        }
        // Push loaded data to all the managers that need it
        foreach(IDataPersistance dataPersistanceObject in this._dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(this._gameData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SaveGame()
    {
        // Pull data from all the managers that need it
        foreach (IDataPersistance dataPersistanceObject in this._dataPersistanceObjects)
        {
            dataPersistanceObject.SaveData(ref this._gameData);
        }

        // Save any data that needs to be saved to a file using the data handler
        _fileDataHandler.Save(this._gameData);
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
