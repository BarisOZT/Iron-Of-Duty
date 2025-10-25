using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogTrigger2 : MonoBehaviour
{
    [Header("Dialog Ayarlar�")]
    public DialogLine[] dialogLines;
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerNameText;
    public Image portraitImage, Etusu;
    public float letterDelay = 0.03f;

    private int currentLine = 0;
    private bool playerInRange = false;
    private Coroutine typingCoroutine;

    public GameObject tus, resim, kap�;

    // Yeni de�i�kenler
    private bool dialogCompleted = false;
    private bool doorOpened = true; // kap�n�n ba�lang�� durumu

    void Start()
    {
        // Daha �nce dialog tamamland� m�?
        dialogCompleted = PlayerPrefs.GetInt("dialogCompleted2", 0) == 1;

        // Kap�n�n durumu
        doorOpened = PlayerPrefs.GetInt("doorOpened2", 1) == 1;
        kap�.SetActive(doorOpened);
    }

    void Update()
    {
        if (dialogCompleted) return;

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogPanel.activeInHierarchy)
            {
                dialogPanel.SetActive(true);
                Time.timeScale = 0f;
                ShowLine();
            }
            else
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                    dialogText.text = dialogLines[currentLine].line;
                    typingCoroutine = null;
                }
                else
                {
                    currentLine++;
                    if (currentLine < dialogLines.Length)
                    {
                        ShowLine();
                    }
                    else
                    {
                        // Diyalog bitti
                        dialogPanel.SetActive(false);
                        currentLine = 0;
                        Time.timeScale = 1f;

                        // Dialog tamamland� olarak i�aretle
                        dialogCompleted = true;
                        PlayerPrefs.SetInt("dialogCompleted2", 1);
                        PlayerPrefs.Save();

                        // Silah 1 a��l�yor
                        PlayerPrefs.SetInt("s1Unlocked", 1);
                        PlayerPrefs.Save();

                        tus.SetActive(true);
                        resim.SetActive(true);

                        // Kap�y� kapat ve kaydet
                        kap�.SetActive(false);
                        PlayerPrefs.SetInt("doorOpened2", 0);
                        PlayerPrefs.Save();

                        PlayerManager playerManager = FindObjectOfType<PlayerManager>();
                        if (playerManager != null)
                        {
                            playerManager.s1.SetActive(true);
                        }
                    }
                }
            }
        }

        Etusu.gameObject.SetActive(playerInRange && !dialogCompleted);
    }

    private void ShowLine()
    {
        speakerNameText.text = dialogLines[currentLine].speakerName;
        portraitImage.sprite = dialogLines[currentLine].portrait;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(dialogLines[currentLine].line));
    }

    private IEnumerator TypeLine(string line)
    {
        dialogText.text = "";
        foreach (char c in line)
        {
            dialogText.text += c;
            yield return new WaitForSecondsRealtime(letterDelay);
        }
        typingCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!dialogCompleted && other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!dialogCompleted && other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogPanel.SetActive(false);
            currentLine = 0;
        }
    }
}
