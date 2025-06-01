using UnityEngine;

namespace Scenes
{
   public class ReturnToMainMenu : MonoBehaviour
   { 
      public void ReturnToMainMenuButton()
      {
         Pause.Instance.ResetPauseStateForNextScene();
         Loader.Load(Loader.Scene.MainMenu);
      }
   }
}
