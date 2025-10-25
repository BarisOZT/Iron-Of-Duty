using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public Image healthBar; // Inspector’da atayacaksýn

    public SpriteRenderer playerSprite; // Karakterin SpriteRenderer'i
    public Color damageColor = Color.red; // Hasar alýnca renk
    public float flashDuration = 0.2f;    // Ne kadar kýrmýzý kalacak

    private Color originalColor = Color.white;

    public GameObject s1, s2, s3, deadPanel;


    private void Start()
    {
        Time.timeScale = 1;
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deadPanel != null)
            deadPanel.SetActive(false); // Baþlangýçta gizli

        

        
    }
    

    private void Update()
    {
        // Sadece unlock edilmiþ olanlar kullanýlabilir
        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerPrefs.GetInt("s1Unlocked", 0) == 1)
        {
            s1.SetActive(true);
            s2.SetActive(false);
            s3.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && PlayerPrefs.GetInt("s2Unlocked", 0) == 1)
        {
            s1.SetActive(false);
            s2.SetActive(true);
            s3.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && PlayerPrefs.GetInt("s3Unlocked", 0) == 1)
        {
            s1.SetActive(false);
            s2.SetActive(false);
            s3.SetActive(true);
        }
    }

    // Hasar alma
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (playerSprite != null)
            StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            SFXManager.Instance.Gameover();
            Die();
        }
            

    }

    private System.Collections.IEnumerator DamageFlash()
    {
        playerSprite.color = damageColor;
        yield return new WaitForSeconds(flashDuration);
        playerSprite.color = originalColor;
    }

    // Can yenileme
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.fillAmount = currentHealth / maxHealth;
    }

    void Die()
    {
        
        deadPanel.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TakeDamage(10f);
        }
        if (collision.CompareTag("enemyBullet"))
        {
            TakeDamage(5f);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("enemyBulletBig"))
        {
            TakeDamage(10f);
            Destroy(collision.gameObject);
        }
    }
}
