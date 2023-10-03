using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject item;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (item != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (item.TryGetComponent<InteractableItem>(out InteractableItem interactableItem))
                {
                    interactableItem.Use();
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (item.TryGetComponent<Reloadable>(out Reloadable reloadable))
                {
                    reloadable.Reload();
                }
            }
        }

    }
}
