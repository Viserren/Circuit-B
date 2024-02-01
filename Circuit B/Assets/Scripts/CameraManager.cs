using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour, IDataPersistance
{
    public static CameraManager Instance { get; private set; }

    AreaCamera _currentCamera;

    MenuCamera _mainMenuCamera;

    //[SerializeField] CinemachineSmoothPath _startPath;
    //CinemachineSmoothPath _currentPath;
    //CinemachineTrackedDolly _cameraTrack;
    //[SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] List<AreaCamera> _cameras;
    string _currentLocation;

    public List<AreaCamera> Cameras { get { return _cameras; } }

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
        _mainMenuCamera = FindObjectOfType<MenuCamera>();

        Cameras.AddRange(FindObjectsByType<AreaCamera>(FindObjectsSortMode.None));

        for (int i = 0; i < Cameras.Count; i++)
        {
            Cameras[i].CreateCamera(i + 1);
            Cameras[i].thisArea.virtualCamera.Priority = 0;
        }

        if(GameStateManager.Instance.CurrentState == GameStateManager.Instance.StateFactory.MainMenu())
        {
            MainMenuCamera(true);
        }
    }

    public void LoadCameras()
    {
        Cameras.AddRange(FindObjectsByType<AreaCamera>(FindObjectsSortMode.None));

        for (int i = 0; i < Cameras.Count; i++)
        {
            Cameras[i].CreateCamera(i + 1);
            Cameras[i].thisArea.virtualCamera.Priority = 0;
        }
    }

    public void MainMenuCamera(bool value, GameBaseState state = null)
    {
        _mainMenuCamera = FindObjectOfType<MenuCamera>();
        if (state != null)
        {
            //Debug.Log(state.ToString());
        }
        if (_mainMenuCamera)
        {
            if (value)
            {
                _mainMenuCamera.thisArea.virtualCamera.Priority = 100;
            }
            else
            {
                _mainMenuCamera.thisArea.virtualCamera.Priority = 0;
            }
        }
    }

    public void ChangeCamera(int newCamera, string newLocation)
    {
        if (_currentCamera)
        {
            _currentCamera.thisArea.virtualCamera.Priority = 0;
        }

        _currentLocation = newLocation;
        _currentCamera = Cameras[newCamera - 1];
        _currentCamera.thisArea.virtualCamera.Priority = 10;
    }

    public void LoadData(GameData gameData)
    {
        _currentLocation = gameData.currentLocation;

        AreaCamera cam = _cameras.Find(r => r.thisArea.cameraName == _currentLocation);
        ChangeCamera(cam.thisArea.cameraNumber, _currentLocation);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.currentLocation = _currentLocation;
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


