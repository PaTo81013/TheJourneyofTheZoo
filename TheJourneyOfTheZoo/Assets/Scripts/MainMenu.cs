using UnityEngine;
public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        Loader.Load(Loader.Scene.Playground);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
