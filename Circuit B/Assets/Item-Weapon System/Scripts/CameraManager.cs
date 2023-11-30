using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }


    //[SerializeField] CinemachineSmoothPath _startPath;
    //CinemachineSmoothPath _currentPath;
    //CinemachineTrackedDolly _cameraTrack;
    //[SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] List<CameraInfo> _cameras;

    public List<CameraInfo> Cameras { get { return _cameras; } set { _cameras = value;} }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //_cameraTrack = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        //_currentPath = _startPath;
        //_cameraTrack.m_Path = _currentPath;
    }
    private void Start()
    {

    }

    /*public void CleanCameras()
    {
        foreach (var cam in _cameras)
        {
            if (cam == null)
            {
                _cameras.Remove(cam);
            }
        }

    }*/


    public void ChangeCamera(int newCamera)
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            if (i == newCamera)
            {
                _cameras[i].virtualCamera.Priority = 10;
            }
            else
            {
                _cameras[i].virtualCamera.Priority = 0;
            }

            if(i == 3)
            {
                _cameras[i].virtualCamera.Priority = 100;
            }
        }
    }

    //public void ChangePath(CinemachineSmoothPath newPath)
    //{
    //    _currentPath = newPath;
    //    _cameraTrack.m_Path = _currentPath;
    //}

    //public void ResetPath()
    //{
    //    _currentPath = _startPath;
    //    _cameraTrack.m_Path = _currentPath;
    //}
}


