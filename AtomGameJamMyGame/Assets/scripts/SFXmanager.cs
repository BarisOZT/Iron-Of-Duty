using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private AudioSource source;
    public AudioClip shoot, roket, patlama, ýþýn, gameover, win;


    private Coroutine beamCoroutine;

    public void StartBeam()
    {
        if (beamCoroutine == null) // zaten çalýyorsa yeniden baþlatma
            beamCoroutine = StartCoroutine(BeamLoop());
    }

    public void StopBeam()
    {
        if (beamCoroutine != null)
        {
            StopCoroutine(beamCoroutine);
            beamCoroutine = null;
        }
    }

    private IEnumerator BeamLoop()
    {
        while (true)
        {
            PlaySFX(ýþýn); // ýþýn sesini çal
            yield return new WaitForSeconds(ýþýn.length); // klibin süresi kadar bekle
        }
    }


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;

        // Kayýtlý sesi yükle
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        source.volume = savedVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
    public void Shoot()
    {
        PlaySFX(shoot);
    }
    public void Roket()
    {
        PlaySFX(roket);
    }
    public void Patlama()
    {
        PlaySFX(patlama);
    }
    public void Gameover()
    {
        PlaySFX(gameover);
    }
    public void Win()
    {
       PlaySFX(win);
    }
}
