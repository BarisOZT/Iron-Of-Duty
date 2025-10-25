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
        // Y koordinatýný -100 ile çarpýp integer yapýyoruz
        sr.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
    }
}
