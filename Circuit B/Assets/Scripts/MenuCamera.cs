using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] CameraInfo _thisArea;
    public CameraInfo thisArea { get { return _thisArea; } }
    // Start is called before the first frame update
    void Awake()
    {
        _thisArea = new CameraInfo(0, gameObject.name, null, GetComponent<CinemachineVirtualCamera>());
    }
}
