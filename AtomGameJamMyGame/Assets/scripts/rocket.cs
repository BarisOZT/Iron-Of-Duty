using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Patlama Ayarlarý")]
    public float explosionRadius = 2f;       // Patlama yarýçapý
    public float explosionDamage = 50f;      // Patlama hasarý
    public GameObject explosionEffect;       // Patlama efekti prefab'ý

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Explode();
        }
    }

    void Explode()
    {
        // Patlama efekti
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            SFXManager.Instance.Patlama();
        }

        // Patlama alanýndaki düþmanlarý bul
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }

    // Patlama alanýný sahnede görmek için (Editor)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
