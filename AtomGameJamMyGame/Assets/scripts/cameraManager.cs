using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;         // Takip edilecek player
    public float smoothSpeed = 5f;   // Smooth hýz
    public Vector2 minBounds;        // Kamera sýnýrlarý
    public Vector2 maxBounds;

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon
        Vector3 desiredPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Smooth hareket
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);

        // Sýnýrlar
        float clampedX = Mathf.Clamp(smoothedPos.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(smoothedPos.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, clampedY, smoothedPos.z);
    }
}
