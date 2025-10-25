using UnityEngine;
using UnityEngine.UI;

public class gunshoot : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public GameObject rocketPrefab;
    public Transform firePoint;
    public float rocketSpeed = 15f;
    public int maxRockets = 5;          // Tek seferde kullanılabilir mermi
    private int currentRockets;

    [Header("Baraj / UI")]
    public Image reloadBar;
    public float reloadTime = 2f;
    private float reloadTimer = 0f;
    private bool isReloading = false;

    [Header("Çoklu Mermi Ayarları")]
    public int rocketsPerShot = 3;      // Aynı anda kaç mermi atılacak
    public float angleStep = 10f;       // Mermeler arasındaki açı

    void Start()
    {
        currentRockets = maxRockets;
        if (reloadBar != null)
        {
            reloadBar.fillAmount = 0f;
            reloadBar.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Atış
        if (Input.GetMouseButtonDown(0) && !isReloading)
        {
            if (currentRockets > 0)
            {
                Shoot();
                currentRockets--;

                if (currentRockets <= 0)
                    StartReload();
            }
        }

        // Manuel reload
        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentRockets < maxRockets)
        {
            StartReload();
        }

        // Yeniden dolum
        if (isReloading)
        {
            reloadTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(reloadTimer / reloadTime);

            if (reloadBar != null)
            {
                reloadBar.gameObject.SetActive(true);
                reloadBar.fillAmount = 1f - progress;
            }

            if (reloadTimer >= reloadTime)
                FinishReload();
        }
    }

    void Shoot()
    {
        SFXManager.Instance.Shoot();
        // Kaç mermi atılacaksa ortadan dağılıyoruz
        float totalAngle = angleStep * (rocketsPerShot - 1);
        float startAngle = -totalAngle / 2f;
        

        for (int i = 0; i < rocketsPerShot; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, 0, currentAngle);

            GameObject rocket = Instantiate(rocketPrefab, firePoint.position, rotation);
            Rigidbody2D rb = rocket.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = rotation * Vector3.right * rocketSpeed;
        }
    }

    void StartReload()
    {
        isReloading = true;
        reloadTimer = 0f;

        if (reloadBar != null)
        {
            reloadBar.gameObject.SetActive(true);
            reloadBar.fillAmount = 1f;
        }
    }

    void FinishReload()
    {
        isReloading = false;
        currentRockets = maxRockets;

        if (reloadBar != null)
            reloadBar.gameObject.SetActive(false);
    }
}
