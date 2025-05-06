using UnityEngine;

public class Pause : MonoBehaviour
{ 
    private static bool _ispaused = false;
    public GameObject pauseCanvas;
 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            _ispaused = !_ispaused;
            Time.timeScale = _ispaused ? 0 : 1;
            pauseCanvas.SetActive(_ispaused);
        }
    }
}
