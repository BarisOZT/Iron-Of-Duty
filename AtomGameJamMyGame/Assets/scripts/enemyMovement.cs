using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    private Transform target;   // Hedef hem kule hem player olabilir

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        FindNewTarget(); // Baþlangýçta en yakýn hedefi bul
    }

    void Update()
    {
        // Hedef yoksa tekrar ara
        if (target == null)
        {
            FindNewTarget();

            if (animator != null)
                animator.SetBool("isWalking", false);
            return;
        }

        // Hedefe doðru hareket
        Vector2 dir = (target.position - transform.position).normalized;

        if (Vector2.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position += (Vector3)dir * speed * Time.deltaTime;

            if (animator != null)
                animator.SetBool("isWalking", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isWalking", false);
        }
    }

    void FindNewTarget()
    {
        // Hem kuleleri hem player’ý bul
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Hedefleri tek listeye topla
        GameObject[] allTargets = new GameObject[towers.Length + players.Length];
        towers.CopyTo(allTargets, 0);
        players.CopyTo(allTargets, towers.Length);

        if (allTargets.Length > 0)
        {
            GameObject closest = GetClosestTarget(allTargets);
            target = closest != null ? closest.transform : null;
        }
        else
        {
            target = null; // hiç hedef yoksa boþ
        }
    }

    GameObject GetClosestTarget(GameObject[] targets)
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject t in targets)
        {
            if (t == null) continue;

            float dist = Vector3.Distance(currentPos, t.transform.position);
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }

        return closest;
    }
}
