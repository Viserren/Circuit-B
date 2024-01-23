using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateLoadButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _saveTitle;
    [SerializeField] TextMeshProUGUI _location;
    [SerializeField] TextMeshProUGUI _timeDate;
    string _uuid;
    Button _self;
    
    public void Populate(string uuid, string saveTitle, string location, string date)
    {
        _uuid = uuid;
        _saveTitle.text = saveTitle;
        _location.text = location;
        _timeDate.text = $"{date}";

        _self = GetComponent<Button>();
        _self.onClick.AddListener(() => DataPersistanceManager.Instance.LoadGame(_uuid));
        _self.onClick.AddListener(() => GameStateManager.Instance.LoadingGame = true);
    }
}
