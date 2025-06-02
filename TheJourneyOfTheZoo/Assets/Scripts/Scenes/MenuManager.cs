using UnityEngine;

namespace Scenes
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject settingsCanvas;
        public GameObject mainMenuCanvas;
    
        public void Play()
        {
            Loader.Load(Loader.Scene.Lobby);
        }
        
        public void Settings()
        {
            mainMenuCanvas.SetActive(false);
            settingsCanvas.SetActive(true);
        }
    
        public void BackToMainMenu()
        {
            mainMenuCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
        }

        public void Credits()
        {
            Loader.Load(Loader.Scene.Credits);
        }
        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
