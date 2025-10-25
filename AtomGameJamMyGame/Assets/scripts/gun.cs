using UnityEngine;

public class GunFollowMouse : MonoBehaviour
{
    public Transform player; // karakter transform'u
    public Vector3 offset;   // player'a göre offset

    private SpriteRenderer sr;
    private Vector3 baseScale;

    void Start()
    {
        // silahýn player'ýn child'ý olsun (isteðe baðlý). Eðer zaten child deðilse de çalýþýr.
        transform.SetParent(player);
        transform.localPosition = offset;

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();

        // baseScale, sprite'ýn normal ölçeði (Inspector'da ayarla)
        baseScale = transform.localScale;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
