using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BossDeathSequence : MonoBehaviour
{
    [Header("Boss Health UI")]
    public Image bossHealthImage; // Boss�un can bar� (fillAmount 0 olunca boss �l�)

    [Header("UI References")]
    public Image fadeImage; // Kararma i�in UI Image
    public TextMeshProUGUI endText; // "SON"
    public TextMeshProUGUI creditsText; // Yap�mc� yaz�s�
    public Button exitButton; // ��k�� butonu

    [Header("Durations")]
    public float fadeDuration = 2f;
    public float textDelay = 1.5f;

    [Header("Credits Scroll Settings")]
    public float creditsScrollDuration = 10f; // Credits yaz�s�n�n akma s�resi
    public float creditsScrollDistance = 500f; // Credits yaz�s�n�n akaca�� mesafe

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
            t += Time.unscaledDeltaTime; // timeScale = 0 olsa bile �al���r
            float alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        // 3) "SON" yaz�s�
        endText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(textDelay);

        // 4) Crossfade m�zik
        MusicManager.Instance.PlayEndingMusic(2f);

        yield return new WaitForSecondsRealtime(textDelay);

        // 5) Credits ak���
        endText.gameObject.SetActive(false);
        creditsText.gameObject.SetActive(true);

        Vector3 startPos = creditsText.transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * creditsScrollDistance;
        t = 0f;
        while (t < creditsScrollDuration)
        {
            t += Time.unscaledDeltaTime; // UI ak��� timeScale = 0 olsa bile �al���r
            creditsText.transform.localPosition = Vector3.Lerp(startPos, endPos, t / creditsScrollDuration);
            yield return null;
        }

        // 6) ��k�� butonu
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
