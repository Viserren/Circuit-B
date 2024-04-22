using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCamera : MonoBehaviour
{
    [SerializeField] Canvas _canvas;
    [SerializeField] Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _canvas = GetComponent<Canvas>();
        _canvas.worldCamera = _camera;
    }

    private void Update()
    {
        if (_camera != null && _canvas != null)
        {
            _canvas.transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
        }
    }
}
