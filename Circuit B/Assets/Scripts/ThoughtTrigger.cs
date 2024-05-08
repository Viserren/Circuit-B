using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtTrigger : MonoBehaviour
{
    [SerializeField] string _thoughtText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerStateManager>(out PlayerStateManager playerStateManager))
        {
            ThoughtsManager.Instance.DisplayThought(_thoughtText);
        }
    }
}
