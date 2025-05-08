using TMPro.Examples;
using UnityEngine;

public class Pause : MonoBehaviour
{ 
    private static bool _ispaused = false;
    public GameObject pauseCanvas;
    public GameObject CrosshairGameObject;
    public MonoBehaviour cameraController;
    public Canvas _TimerCanvas;
    public Canvas _UsernameCanvas;
    
    public static bool IsPaused => _ispaused;
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            _ispaused = !_ispaused;
            Time.timeScale = _ispaused ? 0 : 1;
            pauseCanvas.SetActive(_ispaused);
            CrosshairGameObject.SetActive(!_ispaused);
            _TimerCanvas.enabled = !_ispaused;
            _UsernameCanvas.enabled = !_ispaused;
            
            Cursor.lockState = _ispaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = _ispaused;
            
            if(cameraController != null)
            {
                cameraController.enabled = !_ispaused;
            }

        }
    }
}
