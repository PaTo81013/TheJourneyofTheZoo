using UnityEngine;
using UnityEngine.UI;

public class MusicSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.AssignSliders(musicSlider, sfxSlider); // <= ¡Esto conecta todo!
            AudioManager.Instance.UpdateSliders(); // Actualiza los valores guardados
        }
    }
}