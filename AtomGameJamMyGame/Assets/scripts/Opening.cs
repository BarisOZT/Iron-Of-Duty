using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public float displayTime = 2f; // her resim kaç saniye gözüksün
    

    public GameObject gameplayObjects; // oyun objeleri, baþlangýçta kapalý olsun

    void Start()
    {
        gameplayObjects.SetActive(false);
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        image1.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        image1.gameObject.SetActive(false);

        image2.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        image2.gameObject.SetActive(false);

        gameplayObjects.SetActive(true); // oyun baþlýyor
    }
}
