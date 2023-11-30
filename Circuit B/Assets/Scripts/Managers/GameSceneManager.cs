using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void LoadScene(int newScene)
    {
        SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
    }

    public void LoadScene(int newScene, LoadSceneMode mode)
    {
        SceneManager.LoadSceneAsync(newScene, mode);
    }
}
