using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Genel Ayarlar")]
    public float moveSpeed = 2f;
    public float meleeRange = 1f;
    public float meleeDamage = 20f;
    public Transform player;

    [Header("Lazerler / Mermiler")]
    public GameObject smallLaserPrefab; // Küçük mermi
    public GameObject bigLaserPrefab;   // Büyük mermi
    public Transform firePoint;
    public float smallLaserInterval = 1f;
    public float bigLaserInterval = 10f;
    public float radialLaserInterval = 5f;

    [Header("Mermi Hýzý")]
    public float smallLaserSpeed = 5f;
    public float bigLaserSpeed = 3f;

    private bool isPlayerClose = false;
    private bool isEnraged = false; // %50 altý kontrol

    private EnemyHealth enemyHealth; // EnemyHealth scripti

    void Start()
    {
        // Bu objenin üstündeki EnemyHealth scriptini al
        enemyHealth = GetComponent<EnemyHealth>();

        StartCoroutine(ShootSmallLaser());
        StartCoroutine(ShootRadialLaser());
        StartCoroutine(ShootBigLaser());
    }

    void Update()
    {
        MoveTowardsPlayer();

        if (enemyHealth != null)
        {
            // Can %50 altýna inince "Enraged" moda geç
            if (!isEnraged && enemyHealth.currentHealth <= enemyHealth.maxHealth * 0.5f)
            {
                isEnraged = true;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        isPlayerClose = distance <= meleeRange;

        if (!isPlayerClose)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
        }
        else
        {
            // Oyuncuya yakýnken hasar verme
            PlayerManager ph = player.GetComponent<PlayerManager>();
            if (ph != null)
                ph.TakeDamage(meleeDamage * Time.deltaTime);
        }
    }

    IEnumerator ShootSmallLaser()
    {
        while (true)
        {
            if (smallLaserPrefab != null && firePoint != null && player != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                GameObject bullet = Instantiate(smallLaserPrefab, firePoint.position, Quaternion.identity);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = dir * smallLaserSpeed;
            }
            yield return new WaitForSeconds(smallLaserInterval);
        }
    }

    IEnumerator ShootRadialLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(radialLaserInterval);

            if (firePoint != null)
            {
                if (!isEnraged && smallLaserPrefab != null)
                {
                    // Normal mod: küçük mermi 15° aralýklarla
                    for (int i = 0; i < 360; i += 15)
                    {
                        Quaternion rot = Quaternion.Euler(0, 0, i);
                        GameObject bullet = Instantiate(smallLaserPrefab, firePoint.position, rot);

                        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                        if (rb != null)
                            rb.velocity = bullet.transform.right * smallLaserSpeed;
                    }
                }
                else if (isEnraged) // %50 altý: küçük + büyük ayný anda
                {
                    // Küçük mermiler (15°)
                    if (smallLaserPrefab != null)
                    {
                        for (int i = 0; i < 360; i += 15)
                        {
                            Quaternion rot = Quaternion.Euler(0, 0, i);
                            GameObject bullet = Instantiate(smallLaserPrefab, firePoint.position, rot);

                            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                            if (rb != null)
                                rb.velocity = bullet.transform.right * smallLaserSpeed;
                        }
                    }

                    // Büyük mermiler (60°)
                    if (bigLaserPrefab != null)
                    {
                        for (int i = 0; i < 360; i += 60)
                        {
                            Quaternion rot = Quaternion.Euler(0, 0, i);
                            GameObject bullet = Instantiate(bigLaserPrefab, firePoint.position, rot);

                            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                            if (rb != null)
                                rb.velocity = bullet.transform.right * bigLaserSpeed;
                        }
                    }
                }
            }
        }
    }

    IEnumerator ShootBigLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(bigLaserInterval);

            if (bigLaserPrefab != null && firePoint != null && player != null)
            {
                Vector2 dir = (player.position - firePoint.position).normalized;
                GameObject bullet = Instantiate(bigLaserPrefab, firePoint.position, Quaternion.identity);

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                    rb.velocity = dir * bigLaserSpeed;
            }
        }
    }
}
