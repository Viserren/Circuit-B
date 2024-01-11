using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCamera : MonoBehaviour
{
    [SerializeField] CameraInfo _thisArea;

    [SerializeField] bool _solidOutline;

    [SerializeField] BoxCollider _selfCollider;

    public CameraInfo thisArea {  get { return _thisArea; } set { _thisArea = value; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStateManager>(out PlayerStateManager state))
        {
            CameraManager.Instance.ChangeCamera(_thisArea.cameraNumber);
            //Debug.Log("Change Path");
            //CameraManager.Instance.ChangePath(_path);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent<PlayerStateManager>(out PlayerStateManager state) && _path != null)
    //    {
    //        Debug.Log("Reset Path");
    //        CameraManager.Instance.ResetPath();
    //    }
    //}

    public void CreateCamera(int cameraNumber)
    {
        _thisArea = new CameraInfo(cameraNumber, gameObject.name, GetComponentInChildren<CinemachineSmoothPath>(), GetComponentInChildren<CinemachineVirtualCamera>());
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(_selfCollider.bounds.center, transform.rotation, _selfCollider.size);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = Color.green;
        if (_selfCollider != null)
        {
            if (_solidOutline)
            {
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
            }
            else
            {
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            }
        }
    }
}
