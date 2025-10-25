using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Hedef Ayarlar�")]
    public float moveSpeed = 2f;
    public float attackRange = 5f;
    public float fireRate = 1f;
    private float fireCooldown = 0f;

    private Transform target;

    [Header("Mermi Ayarlar�")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;

    void Update()
    {
        FindClosestTarget();

        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            // Hedefe do�ru hareket et
            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            // Ate� et
            fireCooldown -= Time.deltaTime;
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void FindClosestTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        float closestDist = Mathf.Infinity;
        Transform closestTarget = null;

        // Oyuncular� kontrol et
        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestTarget = p.transform;
            }
        }

        // Kuleleri kontrol et
        foreach (GameObject t in towers)
        {
            float dist = Vector2.Distance(transform.position, t.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestTarget = t.transform;
            }
        }

        target = closestTarget;
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null && target != null)
        {
            Vector2 dir = (target.position - firePoint.position).normalized;
            rb.velocity = dir * bulletSpeed;
        }
    }
}
