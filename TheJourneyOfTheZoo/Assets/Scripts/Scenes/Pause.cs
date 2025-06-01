using UnityEngine;

namespace Scenes
{
    public class Pause : MonoBehaviour
    {
        private static bool _isPaused;
        public GameObject pauseCanvas;
        public GameObject crosshairGameObject;
        public MonoBehaviour cameraController;
        public Canvas UICanvas;
        public Canvas UIKillstreak;

        public static bool IsPaused => _isPaused;

        private static void SetPaused(bool state)
        {
            _isPaused = state;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SetPaused(!_isPaused);
                Time.timeScale = _isPaused ? 0 : 1;
                pauseCanvas.SetActive(_isPaused);
                crosshairGameObject.SetActive(!_isPaused);
                cameraController.enabled = !_isPaused;
                UICanvas.enabled = !_isPaused;
                UIKillstreak.enabled = !_isPaused;
                Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = _isPaused;
            }
        }
    }
}