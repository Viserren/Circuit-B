using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MemoryObject : MonoBehaviour
{
    string _memoryName;

    public string MemoryName { get { return _memoryName; } set { _memoryName = value; } }
    //[SerializeField] Vector3 _spawnLocation;
    //bool _hasCollected;

    //public Vector3 spawnLocation { get { return _spawnLocation; } set { _spawnLocation = value; } }
    //public bool hasCollected { get { return _hasCollected; } }

    //public void Spawn()
    //{
    //    Instantiate(gameObject, _spawnLocation, Quaternion.identity);
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStateManager>(out PlayerStateManager playerStateManager))
        {
            MemoryManager.Instance.UnlockMemory(_memoryName);
            //MemoryManager.Instance.UpdateMemoryViewer(_memory);
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
