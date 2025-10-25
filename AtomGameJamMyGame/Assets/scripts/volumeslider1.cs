using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider2 : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        slider.value = savedVolume;

        slider.onValueChanged.AddListener(delegate {
            if (SFXManager.Instance != null)
                SFXManager.Instance.SetVolume(slider.value);
        });
    }
}
