using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }
    int _sceneToLoad;
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
        _sceneToLoad = newScene;
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive).completed += OnceLoaded;
    }

    public void LoadScene(int newScene, LoadSceneMode mode)
    {
        _sceneToLoad = newScene;
        SceneManager.LoadSceneAsync(_sceneToLoad, mode).completed += OnceLoaded;
    }

    void OnceLoaded(AsyncOperation operation)
    {
        if (operation.isDone && _sceneToLoad == 1)
        {
            //Debug.Log("Called");
            CameraManager.Instance.LoadCameras();
            MenuManager.Instance.Menus.FindAll(r => r.MenuType == MenuType.InGame).ForEach(r => { r.IsActive = false; });
            //AudioManager.Instance.play
            //MenuManager.Instance.MainMenuScreen();
            StartCoroutine(FinishedLoading());
        }

    }

    IEnumerator FinishedLoading()
    {
        yield return new WaitForSecondsRealtime(.5f);
        MenuManager.Instance.HideOpeningScreen();
    }
}
