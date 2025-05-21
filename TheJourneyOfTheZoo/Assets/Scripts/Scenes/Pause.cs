using UnityEngine;

namespace Scenes
{
    public class Pause : MonoBehaviour
    { 
        private static bool _isPaused;
        public GameObject pauseCanvas;
        public GameObject crosshairGameObject;
        public MonoBehaviour cameraController;
        public Canvas timerCanvas;
        public Canvas usernameCanvas;
        public Canvas scoreCanvas;
        public static bool IsPaused => _isPaused;
        private static void SetPaused(bool state)
        {
            _isPaused = state;
        }

        void Start()
        {
            SetPaused(false);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseCanvas.SetActive(false);
            crosshairGameObject.SetActive(true);
            timerCanvas.enabled = true;
            usernameCanvas.enabled = true;
            scoreCanvas.enabled = true;

            if (cameraController != null)
                cameraController.enabled = true;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
            {
                SetPaused(!_isPaused);
        
                Time.timeScale = _isPaused ? 0 : 1;
                pauseCanvas.SetActive(_isPaused);
                crosshairGameObject.SetActive(!_isPaused);
                timerCanvas.enabled = !_isPaused;
                usernameCanvas.enabled = !_isPaused;
                scoreCanvas.enabled = !_isPaused;                

                Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = _isPaused;

                if (cameraController != null)
                    cameraController.enabled = !_isPaused;
            }
        }
    }
}
