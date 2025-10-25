using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Ayarlarý")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Can Barý")]
    public Image healthBarImage; // sadece fillAmount güncellenecek

    [Header("Kan Efekti")]
    public GameObject bloodEffectPrefab; // kýsa süreli partikül
    public GameObject bloodDecalPrefab;  // yere kalýcý kan lekesi
    public float decalOffset = 0.1f;     // z-farký veya yerden yükseklik

    [Header("Hasar Renk Efekti")]
    public SpriteRenderer spriteRenderer;   // düþman sprite renderer
    public Color damageColor = Color.red;    // hasar rengi
    public float flashDuration = 0.2f;      // ne kadar kýrmýzý kalacak

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

        // Hasar alýrken renk efekti
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
        // Kýsa süreli kan partikülü
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
            TakeDamage(20f); // örnek hasar
            Destroy(other.gameObject);
        }
    }
}
