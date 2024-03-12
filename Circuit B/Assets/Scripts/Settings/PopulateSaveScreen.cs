using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class PopulateSaveScreen : MonoBehaviour
{
    [SerializeField] PopulateLoadButton _saveButtonPrefab;
    [SerializeField] GameObject _viewport;

    List<PopulateLoadButton> _loadButtons = new List<PopulateLoadButton>();

    public void Populate()
    {
        DataPersistanceManager.Instance.LoadData();
        foreach (GameData gameData in DataPersistanceManager.Instance.GameDatas)
        {
            if (!_loadButtons.Find(r => r.name == gameData.uuid))
            {
                PopulateLoadButton temp = Instantiate(_saveButtonPrefab, _viewport.transform);
                temp.name = gameData.uuid;
                temp.GetComponent<PopulateLoadButton>().Populate(gameData.uuid, gameData.saveName, Regex.Replace(gameData.currentLocation, "Area", ""), gameData.dateLastSaved.ToString());
                _loadButtons.Add(temp);
            }
            else if(_loadButtons.Find(r => r.name == gameData.uuid))
            {
                PopulateLoadButton temp = _loadButtons.Find(r => r.name == gameData.uuid);
                temp.GetComponent<PopulateLoadButton>().Populate(gameData.uuid, gameData.saveName, Regex.Replace(gameData.currentLocation, "Area", ""), gameData.dateLastSaved.ToString());
            }
        }

        _loadButtons = _loadButtons.OrderByDescending(r => r.date).ToList();
        for (int i = 0; i < _loadButtons.Count; i++)
        {
            _loadButtons[i].transform.SetSiblingIndex(i);
        }
    }
}
