using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    [Header("Kule Can Ayarlarý")]
    public float maxHealth = 200f;
    private float currentHealth;

    [Header("Can Barý")]
    public Image healthBarImage;

    [Header("Hasar Ayarý")]
    public float damagePerSecond = 10f; // düþman kulede durduðu sürede saniye baþýna hasar

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
        // Kule yýkýldýðýnda yapýlacak iþlemler
        Debug.Log("Kule yýkýldý!");
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
