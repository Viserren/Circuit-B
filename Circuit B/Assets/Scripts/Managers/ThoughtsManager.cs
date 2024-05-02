using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThoughtsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    public static ThoughtsManager Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void DisplayThought(string text)
    {
        MenuManager.Instance.ShowScreenButton("Interactive Thoughts");
        if(_text.TryGetComponent<TextAnimator>(out TextAnimator textAnimator))
        {
            textAnimator.VisibleTextAmount = 0;
            _text.text = text;
        }
    }

    public void HideThought()
    {
        MenuManager.Instance.HideScreenButton("Interactive Thoughts");
        if (_text.TryGetComponent<TextAnimator>(out TextAnimator textAnimator))
        {
            textAnimator.VisibleTextAmount = 0;
            _text.text = "";
        }
    }
}
