using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
    Mesh _mesh;
    Vector3[] _vertices;
    int[] _triangles;

    private void Awake()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Update()
    {
        MakeMeshData();
        CreateMesh();
    }

    void MakeMeshData()
    {
        _vertices = new Vector3[] {new Vector3(0,MeshValues.Instance.YValue,0),new Vector3(0,0,1), new Vector3(1,0,0), new Vector3(1,0,1)};

        _triangles = new int[] { 0, 1, 2, 2, 1, 3 };
    }

    void CreateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;

        _mesh.RecalculateNormals();
    }
}