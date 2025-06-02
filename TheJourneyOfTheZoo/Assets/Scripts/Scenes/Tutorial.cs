using UnityEngine;

public class Tutorial : MonoBehaviour
{
   public Canvas TutorialCanvas;

   public void Start()
   {
      TutorialCanvas.enabled = true;
   }
   
   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         TutorialCanvas.enabled = false;
      }
   }
}