using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class MusicSettings : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => AudioManager.Instance != null);
            yield return new WaitForEndOfFrame();
        
            if (musicSlider == null)
                musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
            if (sfxSlider == null)
                sfxSlider = GameObject.Find("SfxSlider")?.GetComponent<Slider>();

            if (musicSlider == null || sfxSlider == null)
            {
                Debug.LogWarning("No se encontraron sliders de música o efectos.");
                yield break;
            }

            AudioManager.Instance.AssignSliders(musicSlider, sfxSlider);
            AudioManager.Instance.UpdateSliders();
        }
    }
}