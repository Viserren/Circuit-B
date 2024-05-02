using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextAnimator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    [Range(0f, 1f)]
    [SerializeField] float _visibleTextAmount;

    public float VisibleTextAmount { get { return _visibleTextAmount; } set {  _visibleTextAmount = value; } }

    private void Start()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.maxVisibleCharacters = Mathf.CeilToInt(_visibleTextAmount * _textMeshPro.textInfo.characterCount);
    }
}
