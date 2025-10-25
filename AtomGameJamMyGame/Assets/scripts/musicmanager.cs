using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Clips")]
    public AudioClip normalMusic;   // Oyun boyunca çalan müzik
    public AudioClip endingMusic;   // Boss ölünce çalacak müzik

    private AudioSource source;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        source.volume = savedVolume;
        PlayNormalMusic();
    }

    public void SetVolume(float volume)
    {
        source.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void PlayNormalMusic()
    {
        if (normalMusic != null)
        {
            source.clip = normalMusic;
            source.loop = true;
            source.Play();
        }
    }

    // Normal -> End music crossfade
    public void PlayEndingMusic(float fadeDuration = 2f)
    {
        StartCoroutine(CrossfadeToEndingMusic(fadeDuration));
    }

    private IEnumerator CrossfadeToEndingMusic(float fadeDuration)
    {
        float startVolume = source.volume;
        float t = 0f;

        // Normal müziði yavaþça kapat
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        source.clip = endingMusic;
        source.loop = false;
        source.Play();

        // Ending müzik için fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0f, startVolume, t / fadeDuration);
            yield return null;
        }

        source.volume = startVolume; // garanti
    }

}
