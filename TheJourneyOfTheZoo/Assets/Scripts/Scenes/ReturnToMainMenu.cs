using UnityEngine;

namespace Scenes
{
   public class ReturnToMainMenu : MonoBehaviour
   {
      public void ReturnToMainMenuButton()
      {
         Loader.Load(Loader.Scene.MainMenu);
      }
   }
}
