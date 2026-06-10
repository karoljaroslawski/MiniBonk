using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;

    [HideInInspector]
    public float speed;

    private Transform player;

    private float attackTimer;

    Animator animator;

   [SerializeField] private float rotationOffset = 0f;

    void Start()
    {
        speed = baseSpeed;

        player =
            GameObject
            .Find("Player")
            .transform;
        animator = GetComponent<Animator>();
        if (animator == null ) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    direction
                ) *
                Quaternion.Euler(
                    0,
                    rotationOffset,
                    0
                );
        }

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                player.position-Vector3.up,
                speed * Time.deltaTime
            );
        
        if(animator != null) animator.SetFloat("speed", speed);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= 1f)
            {
                attackTimer = 0f;

                PlayerHealth health =
                    collision.gameObject.GetComponent<PlayerHealth>();

                if (health != null)
                {
                    if (animator != null) animator.SetTrigger("attack");
                    health.TakeDamage(10);
                }
            }
        }
    }
}