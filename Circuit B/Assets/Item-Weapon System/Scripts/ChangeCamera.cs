using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] Area _selectedArea;

    CinemachineSmoothPath _path;

    [SerializeField] bool _solidOutline;

    private void Start()
    {
        _path = GetComponentInChildren<CinemachineSmoothPath>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStateManager>(out PlayerStateManager state))
        {
            CameraManager.Instance.ChangeCamera((int)_selectedArea);
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

    private void OnDrawGizmos()
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
        Gizmos.matrix = rotationMatrix;
        Gizmos.color = Color.green;
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

[System.Serializable]
public enum Area
{
    House = 0,
    Observatory = 1,
    City = 2
}
