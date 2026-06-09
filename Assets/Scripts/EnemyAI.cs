using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float baseSpeed = 2f;

    [HideInInspector]
    public float speed;

    private Transform player;

    private float attackTimer;

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
                    health.TakeDamage(10);
                }
            }
        }
    }
}