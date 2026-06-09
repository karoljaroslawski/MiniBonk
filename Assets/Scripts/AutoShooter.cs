using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    public float fireRate = 0.5f;

    private float timer;

    public int damage = 10;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fireRate)
        {
            timer = 0f;

            Shoot();
        }
    }

    void Shoot()
    {
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy == null)
            return;

        GameObject bullet =
            Instantiate(
                bulletPrefab,
                transform.position + Vector3.up,
                Quaternion.identity
            );

        Vector3 direction =
            nearestEnemy.transform.position -
            transform.position;

        Bullet bulletScript =
            bullet.GetComponent<Bullet>();

        bulletScript.SetDirection(direction);

        bulletScript.damage = damage;
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        GameObject nearest = null;

        float shortestDistance =
            Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
}