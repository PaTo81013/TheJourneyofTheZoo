using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class Loader
{
    private class LoadingMonoBehaviour: MonoBehaviour { }
    public enum Scene
    {
        LoadingScene,
        Lobby,
        Central,
        Final,
        MainMenu
    }

    private static Action _onLoaderCallback;
    private static AsyncOperation _loadingAsyncOperation;
    
    public static void Load(Scene scene)
    {
        _onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        };
        
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Scene.LoadingScene.ToString());
    }

    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;

        _loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());
        _loadingAsyncOperation.allowSceneActivation = false;

        float minLoadTime = 2f; 
        float timer = 0f;

        while (!_loadingAsyncOperation.isDone)
        {
            timer += Time.unscaledDeltaTime;
            
            if (_loadingAsyncOperation.progress >= 0.9f && timer >= minLoadTime)
            {
                _loadingAsyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    public static float GetLoadingProgress()
    {
        if (_loadingAsyncOperation !=null)
        {
            return _loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }
    
    public static void LoaderCallback ()
    {
        if (_onLoaderCallback == null) return;
        _onLoaderCallback();
        _onLoaderCallback = null;
    }
}
