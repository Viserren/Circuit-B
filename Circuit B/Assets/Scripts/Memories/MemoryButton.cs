using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoryButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _buttonText;

    public TextMeshProUGUI ButtonText { get { return _buttonText; } }
    Memories _memory;

    public Memories Memory { get { return _memory; }  set { _memory = value; } }

    public void CallUpdateMemory()
    {
        if (_memory.HasCollected)
        {
            MemoryManager.Instance.UpdateMemoryViewer(_memory.MemoryName);
        }
    }
}
