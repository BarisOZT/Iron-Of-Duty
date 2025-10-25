using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    [Header("Kule Can Ayarlar�")]
    public float maxHealth = 200f;
    private float currentHealth;

    [Header("Can Bar�")]
    public Image healthBarImage;

    [Header("Hasar Ayar�")]
    public float damagePerSecond = 10f; // d��man kulede durdu�u s�rede saniye ba��na hasar

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            healthBarImage.fillAmount = currentHealth / maxHealth;
        }
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Kule y�k�ld���nda yap�lacak i�lemler
        Debug.Log("Kule y�k�ld�!");
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Her frame damage uygula
            TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyBullet"))
        {
            // Her frame damage uygula
            TakeDamage(5);
            Destroy(collision.gameObject);
        }
    }
}
