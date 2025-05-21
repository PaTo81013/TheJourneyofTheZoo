using UnityEngine;

namespace Scenes
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private Loader.Scene scene;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Loader.Load(scene);
            }
        }
    }
}
