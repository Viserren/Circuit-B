using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraInfo
{
    int _cameraNumber;
    string _cameraName;
    CinemachineSmoothPath _path;
    CinemachineVirtualCamera _virtualCamera;

    public int cameraNumber { get { return _cameraNumber; } }
    public string cameraName { get { return _cameraName; } }
    public CinemachineSmoothPath path { get { return _path; } }
    public CinemachineVirtualCamera virtualCamera { get { return _virtualCamera; } }

    public CameraInfo(int cameraNumber, string cameraName, CinemachineSmoothPath path, CinemachineVirtualCamera virtualCamera)
    {
        _cameraNumber = cameraNumber;
        _cameraName = cameraName;
        _path = path;
        _virtualCamera = virtualCamera;
    }
}
