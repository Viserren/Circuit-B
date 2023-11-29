using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBodyMeshes : MonoBehaviour
{
    [SerializeField] Mesh _originalMesh, _overgrownMesh;

    SkinnedMeshRenderer _mesh;

    private void Start()
    {
        _mesh = GetComponent<SkinnedMeshRenderer>();
        _originalMesh = _mesh.sharedMesh;
    }

    public void ChangeMesh(bool isOvergrown)
    {
        if (_originalMesh != null && _overgrownMesh != null)
        {
            _mesh.sharedMesh = isOvergrown ? _overgrownMesh : _originalMesh;
        }
    }
}
