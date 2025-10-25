using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        slider.value = savedVolume;

        slider.onValueChanged.AddListener(delegate {
            if (MusicManager.Instance != null)
                MusicManager.Instance.SetVolume(slider.value);
        });
    }
}
