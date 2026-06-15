using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    public float speedMult = 1f;

    public int damage = 10;

    public int maxEnemyHits = 1;

    private int enemiesHit;

    private readonly HashSet<EnemyHealth> hitEnemies = new HashSet<EnemyHealth>();

    private Vector3 direction;

    public Material materialSingle;
    public Material materialSpread;
    public Material materialFast;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public void setMaterial(WeaponTypes weaponType)
    {

        switch (weaponType)
        {
            case WeaponTypes.single:
                this.GetComponent<MeshRenderer>().material = materialSingle;
                break;
            case WeaponTypes.shotgun:
                this.GetComponent<MeshRenderer>().material = materialSpread;
                break;
            case WeaponTypes.sniper:
                this.GetComponent<MeshRenderer>().material = materialFast;
                break;
        }
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
        Destroy(gameObject, 2.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("bullet"))
            return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null && !enemy.name.Contains("Ghost"))
        {
            if (hitEnemies.Contains(enemy))
                return;

            hitEnemies.Add(enemy);
            enemy.TakeDamage(damage);
            enemiesHit++;

            if (enemiesHit >= maxEnemyHits)
                Destroy(gameObject);

            return;
        }

        Destroy(gameObject);
    }
}