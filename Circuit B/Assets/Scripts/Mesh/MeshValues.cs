using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshValues : MonoBehaviour
{
    public float YValue;
    public static MeshValues Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
}
