using UnityEngine;

namespace Scenes
{
    public class Credits : MonoBehaviour
    {
        public float speed = 250f;
        private float _endCreditsYPosition = -1080f;
        public RectTransform rectTransform;

        public bool hasEnded = false;

        public void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Update()
        {
            if (hasEnded)
                return;
        
            rectTransform.anchoredPosition += Vector2.down * speed * Time.deltaTime;
        
            if (rectTransform.anchoredPosition.y <= _endCreditsYPosition)
            {
                Debug.Log("Credits ended");
                hasEnded = true;
                Loader.Load(Loader.Scene.MainMenu);
            }
        }
    }
}
