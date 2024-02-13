using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Unity.Mathematics;

public class FileDataHandler
{
    private string _dataDirPath;
    private string _dataFileName;

    public string dataFileName { get { return _dataFileName; } }

    GameData[] _files;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this._dataDirPath = dataDirPath;
        this._dataFileName = dataFileName;
    }

    public FileDataHandler(string dataDirPath)
    {
        this._dataDirPath = dataDirPath;
    }

    public List<GameData> LoadAllFiles()
    {
        string[] files = System.IO.Directory.GetFiles(_dataDirPath, "*.txt");
        List<GameData> loadedData = new List<GameData>();

        if (files.Length > 0)
        {
            foreach (string file in files)
            {
                string fullPath = Path.Combine(_dataDirPath, file);
                Debug.Log($"Loading {fullPath}");
                try
                {
                    // load the data from file
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();
                        }
                    }

                    // deserialize the data into a GameData object
                    //Debug.Log(JsonUtility.FromJson<GameData>(dataToLoad));

                    GameData updatedData = new GameData("","");

                    JsonUtility.FromJsonOverwrite(dataToLoad, updatedData);



                    loadedData.Add(updatedData);
                    Debug.Log("Loaded");
                }
                catch (Exception e)
                {
                    Debug.LogError("Error occured while loading data from file: " + fullPath + "\n" + e);
                }
            }
        }
        return loadedData;
    }

    public GameData Load(string fileNameToLoad)
    {
        _dataFileName = fileNameToLoad;
        // Using Path.Combine to account for different OS's
        string fullPath = Path.Combine(_dataDirPath, $"{_dataFileName}.txt");
        GameData loadedData = DataPersistanceManager.Instance.GameDatas.Find(r => r.uuid == fileNameToLoad);

        return loadedData;
    }

    public void Save(GameData gameData)
    {
        // Using Path.Combine to account for different OS's
        string fullPath = Path.Combine(_dataDirPath, $"{_dataFileName}.txt");

        try
        {
            // create the directory if it doesnt already exist
            if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            // serialize the data into a json string
            string jsonData = JsonUtility.ToJson(gameData, true);

            // write the json string to the file
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonData);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured while saving data to file: " + fullPath + "\n" + e);
        }
    }



    void VerifyLoadedData(GameData gameData)
    {
        
    }
}
