using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BossDeathSequence : MonoBehaviour
{
    [Header("Boss Health UI")]
    public Image bossHealthImage; // Boss’un can barý (fillAmount 0 olunca boss ölü)

    [Header("UI References")]
    public Image fadeImage; // Kararma için UI Image
    public TextMeshProUGUI endText; // "SON"
    public TextMeshProUGUI creditsText; // Yapýmcý yazýsý
    public Button exitButton; // Çýkýþ butonu

    [Header("Durations")]
    public float fadeDuration = 2f;
    public float textDelay = 1.5f;

    [Header("Credits Scroll Settings")]
    public float creditsScrollDuration = 10f; // Credits yazýsýnýn akma süresi
    public float creditsScrollDistance = 500f; // Credits yazýsýnýn akacaðý mesafe

    private bool isDead = false;

    void Start()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0);

        endText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isDead && bossHealthImage.fillAmount <= 0f)
        {
            isDead = true;
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        // 1) Oyunu durdur
        Time.timeScale = 0f;

        // 2) Kararma
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime; // timeScale = 0 olsa bile çalýþýr
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 3) "SON" yazýsý
        endText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(textDelay);

        // 4) Crossfade müzik
        MusicManager.Instance.PlayEndingMusic(2f);

        yield return new WaitForSecondsRealtime(textDelay);

        // 5) Credits akýþý
        endText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(true);

        Vector3 startPos = creditsText.transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * creditsScrollDistance;
        t = 0f;
        while (t < creditsScrollDuration)
        {
            t += Time.unscaledDeltaTime; // UI akýþý timeScale = 0 olsa bile çalýþýr
            creditsText.transform.localPosition = Vector3.Lerp(startPos, endPos, t / creditsScrollDuration);
            yield return null;
        }

        // 6) Çýkýþ butonu
        creditsText.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(true);
    }




    public void mainmenuOpen()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
    }
}
