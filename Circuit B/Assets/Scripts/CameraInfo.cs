using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInfo : MonoBehaviour
{
    public int cameraNumber;
    public string cameraName;
    public CinemachineVirtualCamera virtualCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        //grab CameraInfo class and add to CameraList in Manager
        CameraManager.Instance.Cameras[cameraNumber - 1] = this;
    }

}
