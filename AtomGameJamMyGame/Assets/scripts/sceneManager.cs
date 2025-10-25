using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    private const string progressKey = "GameProgress"; // PlayerPrefs key
    public GameObject pausepanel;

    void Start()
    {
        // Oyunun ilk ba�lama kontrol�
        if (!PlayerPrefs.HasKey(progressKey))
        {
            PlayerPrefs.SetInt(progressKey, 1); // �lk defa ba�lat�l�yorsa level 1 aktif
            PlayerPrefs.Save();
        }
    }

    // Ana men�de Start butonu
    public void StartGame()
    {
        int currentProgress = PlayerPrefs.GetInt(progressKey, 1);

        // Progress de�erine g�re sahneyi y�kle
        switch (currentProgress)
        {
            case 1:
                SceneManager.LoadScene(1);
                break;
            case 2:
                SceneManager.LoadScene(2);
                break;
            case 3:
                SceneManager.LoadScene(3);
                break;
            case 4:
                SceneManager.LoadScene(4);
                break;
            case 5:
                SceneManager.LoadScene(5);
                break;
            case 6:
                SceneManager.LoadScene(5);
                break;
            default:
                SceneManager.LoadScene(1);
                break;
        }
    }

    // Tekrar dene
    public void retry()
    {
        int currentProgress = PlayerPrefs.GetInt(progressKey, 1);
        SceneManager.LoadScene(currentProgress);
    }

    // Next
    public void Next()
    {
        int currentProgress = PlayerPrefs.GetInt(progressKey, 1);
        currentProgress++;
        if (currentProgress > 6) currentProgress = 6; // max next4
        PlayerPrefs.SetInt(progressKey, currentProgress);
        PlayerPrefs.Save();

        SceneManager.LoadScene(currentProgress);
    }

    // Ana men�
    public void mainmenu()
    {
        SceneManager.LoadScene(0);
        MusicManager.Instance.PlayNormalMusic();
    }
   public void Pause()
    {
        Time.timeScale = 0;
        pausepanel.SetActive(true);
    }
    public void Continue()
    {
        Time.timeScale = 1;
        pausepanel.SetActive(false);
    }
}
