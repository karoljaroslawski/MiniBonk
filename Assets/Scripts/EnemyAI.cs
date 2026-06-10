using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;

    [HideInInspector]
    public float speed;

    private Transform player;

    private float attackTimer = 0f;

    void Start()
    {
        speed = baseSpeed;

        player =
            GameObject
            .Find("Player")
            .transform;
    }

    void Update()
    {
        attackTimer = Mathf.Max(attackTimer - Time.deltaTime, 0f);

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
                    90,
                    0
                );
        }

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                player.position,
                speed * Time.deltaTime
            );
    }

    void OnCollisionStay(Collision collision)
    {
        if (attackTimer <= 0f)
        {
            PlayerHealth health =
                collision.gameObject.GetComponent<PlayerHealth>();

            if (health != null)
                health.TakeDamage(10);
            attackTimer = 1f;
        }
    }
}