using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Ayarlar�")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Can Bar�")]
    public Image healthBarImage; // sadece fillAmount g�ncellenecek

    [Header("Kan Efekti")]
    public GameObject bloodEffectPrefab; // k�sa s�reli partik�l
    public GameObject bloodDecalPrefab;  // yere kal�c� kan lekesi
    public float decalOffset = 0.1f;     // z-fark� veya yerden y�kseklik

    [Header("Hasar Renk Efekti")]
    public SpriteRenderer spriteRenderer;   // d��man sprite renderer
    public Color damageColor = Color.red;    // hasar rengi
    public float flashDuration = 0.2f;      // ne kadar k�rm�z� kalacak

    private Color originalColor = Color.white;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        // Kan efektleri
        SpawnBloodEffects();

        // Hasar al�rken renk efekti
        if (spriteRenderer != null)
            StartCoroutine(FlashDamageColor());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashDamageColor()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    private void SpawnBloodEffects()
    {
        // K�sa s�reli kan partik�l�
        if (bloodEffectPrefab != null)
        {
            GameObject effect = Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 1f); // 1 saniye sonra yok olsun
        }

        // Yere kan lekesi
        if (bloodDecalPrefab != null)
        {
            Vector3 decalPos = transform.position + Vector3.back * decalOffset;
            Instantiate(bloodDecalPrefab, decalPos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(20f); // �rnek hasar
            Destroy(other.gameObject);
        }
    }
}
