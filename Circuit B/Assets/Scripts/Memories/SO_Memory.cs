using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Memory Data", menuName = "Data/Memory Data")]
public class SO_Memory : ScriptableObject
{
    [SerializeField] string _title;
    [SerializeField] string _description;
    [SerializeField] Sprite _picture;
    //bool _hasCollected;

    public string Title { get { return _title; } }
    public string Description { get { return _description; } }
    public Sprite Picture { get { return _picture; } }
    //public bool HasCollected { get { return _hasCollected; } set { _hasCollected = value; } }

}
