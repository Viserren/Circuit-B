using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File Storage Settings")]
    private string _fileName;


    private GameData _gameData;
    private List<IDataPersistance> _dataPersistanceObjects;
    private List<GameData> _saveDatas;

    private FileDataHandler _fileDataHandler;

    public static DataPersistanceManager Instance { get; private set; }
    public List<GameData> GameDatas { get { return _saveDatas; } }

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

        LoadData();

    }

    public void LoadData()
    {
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath);
        _saveDatas = _fileDataHandler.LoadAllFiles();
    }

    public void NewGame()
    {
        Guid guid = Guid.NewGuid();
        _fileName = $"{guid.ToString()}.txt";
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, this._fileName);
        this._dataPersistanceObjects = FindAllDataPersistanceObjects();
        this._gameData = new GameData(_fileName, "test");

        foreach (IDataPersistance dataPersistanceObject in this._dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(this._gameData);
        }
    }

    public void LoadGame(string fileNameToLoad)
    {
        // Load any saved data from a file using the data handler
        this._fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileNameToLoad);
        this._gameData = _fileDataHandler.Load(fileNameToLoad);
        this._dataPersistanceObjects = FindAllDataPersistanceObjects();

        // If no data was found, initalize the game with defaults
        //if(this._gameData == null)
        //{
        //    Debug.Log("No data was found. Initalizing game with defaults.");
        //    NewGame();
        //}

        // Push loaded data to all the managers that need it
        foreach (IDataPersistance dataPersistanceObject in this._dataPersistanceObjects)
        {
            dataPersistanceObject.LoadData(this._gameData);
        }

        GameStateManager.Instance.DoneLoading = true;
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    public void SaveGame()
    {
        if (_fileDataHandler.dataFileName != null)
        {
            // Pull data from all the managers that need it
            foreach (IDataPersistance dataPersistanceObject in this._dataPersistanceObjects)
            {
                dataPersistanceObject.SaveData(ref this._gameData);
            }

            // Save any data that needs to be saved to a file using the data handler
            _fileDataHandler.Save(this._gameData);
        }
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
