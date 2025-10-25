using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogTrigger5 : MonoBehaviour
{
    [Header("Dialog Ayarlarý")]
    public DialogLine[] dialogLines;
    public GameObject dialogPanel;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerNameText;
    public Image portraitImage, Etusu;
    public float letterDelay = 0.03f;

    [Header("UI & Efektler")]
    public GameObject tus, resim, kapý;
    public Image blackScreenPanel; // Siyah panel, tüm ekraný kaplayan Image

    

    private int currentLine = 0;
    private bool playerInRange = false;
    private Coroutine typingCoroutine;
    public sceneManager sm;

    void Start()
    {
        kapý.SetActive(true);
        tus.SetActive(true);
        resim.SetActive(true);

        if (blackScreenPanel != null)
        {
            Color c = blackScreenPanel.color;
            c.a = 0f;
            blackScreenPanel.color = c;
            blackScreenPanel.gameObject.SetActive(true);
        }
    }

    void Update()
    {
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
                        EndDialog();
                    }
                }
            }
        }

        Etusu.gameObject.SetActive(playerInRange);
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

    private void EndDialog()
    {
        dialogPanel.SetActive(false);
        currentLine = 0;
        Time.timeScale = 1f;

        kapý.SetActive(false);

        if (blackScreenPanel != null)
            StartCoroutine(FadeBlackScreenAndLoadScene());
    }

    private IEnumerator FadeBlackScreenAndLoadScene()
    {
        float duration = 1.5f;
        float elapsed = 0f;
        Color c = blackScreenPanel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / duration);
            blackScreenPanel.color = c;
            yield return null;
        }

        c.a = 1f;
        blackScreenPanel.color = c;

        sm.Next();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogPanel.SetActive(false);
            currentLine = 0;
        }
    }
}
