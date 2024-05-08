using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThoughtsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] int _secondsToDisplay;
    bool _isShowing;
    public static ThoughtsManager Instance { get; private set; }
    public bool IsShowing { get { return _isShowing; } }

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
        if (_text.TryGetComponent<TextAnimator>(out TextAnimator textAnimator))
        {
            textAnimator.VisibleTextAmount = 0;
            _text.text = text;
            StartCoroutine(PlayText(textAnimator));
            _isShowing = true;
        }
    }

    public void HideThought()
    {
        MenuManager.Instance.HideScreenButton("Interactive Thoughts");
        if (_text.TryGetComponent<TextAnimator>(out TextAnimator textAnimator))
        {
            textAnimator.VisibleTextAmount = 0;
            _text.text = "";
            _isShowing = false;
        }
    }

    IEnumerator PlayText(TextAnimator textAnimator)
    {
        int totalTime = _secondsToDisplay * 100;
        float linerTime;
        for (int i = 0; i < totalTime + 1; i++)
        {
            linerTime = (float)i/(float)totalTime;
            Debug.Log($"I Value: {i} {i}/{totalTime}={linerTime}");
            textAnimator.VisibleTextAmount = linerTime;
            yield return new WaitForSeconds(.001f);
        }
    }
}
