using UnityEngine;
using System.Collections;

public class flyingEnemyAi : MonoBehaviour
{
    [HideInInspector]
    public float speed;
    public float baseSpeed = 2f;
    public float hoverHeight = 4f;

    public float maxDistance = 15f;

    public GameObject projectile;
    public Transform shootPoint;

    private Transform player;
    public float shootCooldown = 1.5f;
    public float projectileSpeed = 10f;

    float shootTimer;


    private Animator animator;

    [SerializeField] private float rotationOffset = 0f;

    void Start()
    {
        speed = baseSpeed;
        player =
           GameObject
           .Find("Player")
           .transform;
        animator = GetComponent<Animator>();
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;

        Vector3 toPlayer = player.position - transform.position;
        float distance = toPlayer.magnitude;

        Vector3 direction = toPlayer.normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(direction) *
                Quaternion.Euler(0, rotationOffset, 0);
        }

        Vector3 basePos = transform.position;

        if (distance > maxDistance)
        {
            basePos += direction * speed * Time.deltaTime;
        }

        float hover = hoverHeight + Mathf.Sin(Time.time * 2f) * 0.3f;

        transform.position = new Vector3(
            basePos.x,
            hover,
            basePos.z
        );

        if (distance <= maxDistance && shootTimer <= 0f)
        {
            shootTimer = shootCooldown;
            StartCoroutine(Attack());
            
        }

        if (animator != null)
            animator.SetFloat("speed", direction.magnitude);
    }

    IEnumerator Attack()
    {
        if (animator != null)
            animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.6f);
        Shoot();
    }

    void Shoot()
    {
        if (projectile == null || shootPoint == null) return;

        GameObject proj = Instantiate(projectile, shootPoint.position, Quaternion.identity);

        Vector3 dir = (player.position - shootPoint.position).normalized;

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = dir * projectileSpeed;
        }


    }
}