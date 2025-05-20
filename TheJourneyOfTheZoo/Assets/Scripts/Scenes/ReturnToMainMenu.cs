using UnityEngine;

public class ReturnToMainMenu : MonoBehaviour
{
   public void ReturnToMainMenuButton()
   {
      Loader.Load(Loader.Scene.MainMenu);
   }
}
