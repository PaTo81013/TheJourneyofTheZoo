using UnityEngine;

namespace Scenes
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject settingsCanvas;
        public GameObject mainMenuCanvas;
    
        public void Play()
        {
            AudioManager.Instance.PlaySfx("Click");
            Loader.Load(Loader.Scene.Lobby);
        }
        
        public void Settings()
        {
            AudioManager.Instance.PlaySfx("Click");
            mainMenuCanvas.SetActive(false);
            settingsCanvas.SetActive(true);
        }
        
        public void BackToMainMenu()
        {
            AudioManager.Instance.PlaySfx("Click");
            mainMenuCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
        }

        public void Credits()
        {
            AudioManager.Instance.PlaySfx("Click");
            Loader.Load(Loader.Scene.Credits);
        }

        public void Central()
        {
            Loader.Load(Loader.Scene.Central);
        }

        public void Final()
        {
            Loader.Load(Loader.Scene.Final);
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
