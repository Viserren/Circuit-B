using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Opening ready");
    }

    public void LoadScene()
    {
        GameStateManager.Instance.LoadSceneAfter();
    }

    public void DoneLoading()
    {
        GameStateManager.Instance.DoneSceneLoad();
    }
}
