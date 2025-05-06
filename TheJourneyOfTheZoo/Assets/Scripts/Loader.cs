using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        LoadingScene,
        Playground
    }

    private static Scene _targetScene;

    public static void Load(Scene targetScene)
    {
        _targetScene = targetScene;
        
        switch (targetScene)
        {
            case Scene.MainMenu:
                AudioManager.Instance.PlayMusic("MainMenu");
                break;
            case Scene.LoadingScene:
                AudioManager.Instance.PlayMusic("LoadingScene");
                break;
            case Scene.Playground:
                AudioManager.Instance.PlayMusic("Playground");
                break;
        }
        
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoadTargetScene()
    {
        // Carga la escena objetivo
        SceneManager.LoadScene(_targetScene.ToString());
    }
}