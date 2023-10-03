using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }


    [SerializeField] CinemachineSmoothPath _mainPath;
    CinemachineSmoothPath _currentPath;
    CinemachineTrackedDolly _cameraTrack;
    [SerializeField] CinemachineVirtualCamera _camera;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }


        _cameraTrack = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        _currentPath = _mainPath;
        _cameraTrack.m_Path = _currentPath;
    }

    public void ChangePath(CinemachineSmoothPath newPath)
    {
        _currentPath = newPath;
        _cameraTrack.m_Path = _currentPath;
    }

    public void ResetPath()
    {
        _currentPath = _mainPath;
        _cameraTrack.m_Path = _currentPath;
    }
}
