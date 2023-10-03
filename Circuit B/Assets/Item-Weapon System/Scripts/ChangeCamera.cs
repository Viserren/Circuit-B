using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    CinemachineSmoothPath _path;

    private void Start()
    {
        _path = GetComponentInChildren<CinemachineSmoothPath>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _path != null)
        {
            Debug.Log("Change Path");
            CameraManager.instance.ChangePath(_path);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.instance.ResetPath();
        }
    }
}
