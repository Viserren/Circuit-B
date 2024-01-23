using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulateSaveScreen : MonoBehaviour
{
    [SerializeField] PopulateLoadButton _saveButtonPrefab;
    [SerializeField] GameObject _viewport;

    List<GameObject> _loadButtons = new List<GameObject>();

    public void Populate()
    {
        DataPersistanceManager.Instance.LoadData();
        foreach (GameData gameData in DataPersistanceManager.Instance.GameDatas)
        {
            if (!_loadButtons.Find(r => r.name == gameData.uuid))
            {
                GameObject temp = Instantiate(_saveButtonPrefab.gameObject, _viewport.transform);
                temp.name = gameData.uuid;
                temp.GetComponent<PopulateLoadButton>().Populate(gameData.uuid, gameData.saveName, "Shack", gameData.dateLastSaved.ToString());
                _loadButtons.Add(temp);
            }
        }
    }
}
