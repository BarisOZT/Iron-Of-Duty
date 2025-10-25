using UnityEngine;
using UnityEngine.UI;

public class LaserWeaponSprite : MonoBehaviour
{
    [Header("Lazer Ayarları")]
    public Transform gunTip;           // silahın namlu ucu
    public GameObject laserSprite;     // lazer objesi (SpriteRenderer)
    public float maxDistance = 20f;    // sabit uzunluk (örnek: 20 metre)
    public float laserDamage = 30f;

    [Header("Enerji / Bar")]
    public Image energyBar;
    public float maxEnergy = 100f;
    public float energyDrainRate = 30f;
    public float energyRechargeRate = 15f;

    private float currentEnergy;
    private bool canFire = true;

    void Start()
    {
        currentEnergy = maxEnergy;
        if (laserSprite != null)
            laserSprite.SetActive(false);
    }

    void Update()
    {
        // Enerji geri dolma
        if (!Input.GetMouseButton(0) || currentEnergy <= 0)
        {
            SFXManager.Instance.StopBeam();
            if (laserSprite != null)
                laserSprite.SetActive(false);

            currentEnergy += energyRechargeRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

            if (currentEnergy >= maxEnergy * 0.25f)
                canFire = true;
        }

        // Lazer ateşi
        if (Input.GetMouseButton(0) && canFire && currentEnergy > 0)
        {
            SFXManager.Instance.StartBeam();
            ShootLaser();
            currentEnergy -= energyDrainRate * Time.deltaTime;
            currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

            if (currentEnergy <= 0)
                canFire = false;
        }

        if (energyBar != null)
            energyBar.fillAmount = currentEnergy / maxEnergy;
    }

    void ShootLaser()
    {
        if (laserSprite != null)
            laserSprite.SetActive(true);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - gunTip.position).normalized;

        // Lazer pozisyon ve rotasyon
        laserSprite.transform.position = gunTip.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserSprite.transform.rotation = Quaternion.Euler(0, 0, angle);

        // Lazer uzunluğu = her zaman sabit maxDistance
        laserSprite.transform.localScale = new Vector3(maxDistance, 0.2f, 1);

        // RaycastAll ile sabit mesafede tarama
        RaycastHit2D[] hits = Physics2D.RaycastAll(gunTip.position, direction, maxDistance);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                    enemy.TakeDamage(laserDamage * Time.deltaTime);
            }
        }
    }
}
