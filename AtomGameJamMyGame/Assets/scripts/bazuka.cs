using UnityEngine;
using UnityEngine.UI;

public class BazukaWeapon : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject rocketPrefab;      // Fırlatılacak mermi prefab'ı
    public Transform firePoint;          // Merminin çıkış noktası
    public float rocketSpeed = 15f;      // Mermi hızı
    public int maxRockets = 5;           // Tek seferde atılabilecek mermi
    private int currentRockets;

    [Header("Yükleme / Bar")]
    public Image reloadBar;              // UI bar
    public float reloadTime = 2f;        // tüm mermiler dolması için geçen süre
    private float reloadTimer = 0f;
    private bool isReloading = false;

    void Start()
    {
        currentRockets = maxRockets;
        if (reloadBar != null)
        {
            reloadBar.fillAmount = 0f;
            reloadBar.gameObject.SetActive(false); // başlangıçta gizli
        }
    }

    void Update()
    {
        // Atış
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            if (currentRockets > 0)
            {
                FireRocket();
                currentRockets--;

                if (currentRockets <= 0)
                {
                    isReloading = true;
                    reloadTimer = 0f;
                    if (reloadBar != null)
                    {
                        reloadBar.gameObject.SetActive(true); // reload başlarken aç
                        reloadBar.fillAmount = 1f; // 1’den başlasın
                    }
                }
            }
        }

        // Yeniden dolum
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(reloadTimer / reloadTime);

            // 1 → 0 doğru azalsın
            if (reloadBar != null)
                reloadBar.fillAmount = 1f - progress;

            if (reloadTimer >= reloadTime)
            {
                isReloading = false;
                currentRockets = maxRockets;

                if (reloadBar != null)
                    reloadBar.gameObject.SetActive(false); // reload bitince kapat
            }
        }
    }

    void FireRocket()
    {
        GameObject rocket = Instantiate(rocketPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = firePoint.right * rocketSpeed;
    }
}
