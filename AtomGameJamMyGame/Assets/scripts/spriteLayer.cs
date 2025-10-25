using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortLayer : MonoBehaviour
{
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // Y koordinat�n� -100 ile �arp�p integer yap�yoruz
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}
