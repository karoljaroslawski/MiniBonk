using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    public float speedMult = 1f;

    public int damage = 10;

    private Vector3 direction;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;

        Debug.Log("Direction: " + direction);
    }

    void Update()
    {
        transform.position +=
            direction *
            speed *
            speedMult *
            Time.deltaTime;
    }

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();

        if (enemy != null)
            enemy.TakeDamage(damage);
        if(other.tag!="Player")
            Destroy(gameObject);
    }
}