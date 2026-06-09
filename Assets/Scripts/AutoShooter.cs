using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    private float timer;

    public WeaponManager wm;
    void Update()
    {
        timer += Time.deltaTime;
        foreach (Weapon w in this.wm.activeWeapons)
        {
            Shoot(w, timer);
        }
    }

    void ShootBullet(Weapon w, Vector3 direction, GameObject nearestEnemy)
    {
        GameObject bullet =
        Instantiate(
        bulletPrefab,
        transform.position + Vector3.up,
        Quaternion.identity
        );

        Bullet bulletScript =
            bullet.GetComponent<Bullet>();

        bulletScript.SetDirection(direction);

        bulletScript.damage = w.damage;
        bulletScript.speedMult = w.speedMult;

        bulletScript.setMaterial(w.weaponType);
    }

    void Shoot(Weapon w, float timer)
    {
        if (timer - w.lastShot < w.fireRate)
            return;
        w.lastShot = timer;
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy == null)
            return;

        if (w.weaponType == WeaponTypes.shotgun) {
            for (int i = 0; i < w.bulletNumber; i++)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float spread = 45f/w.bulletNumber;
                Quaternion rotation = Quaternion.AngleAxis((i-w.bulletNumber/2)*spread, Vector3.up);
                ShootBullet(w, rotation * direction, nearestEnemy);
            }
        }
        else
        {
            Vector3 direction =
                    nearestEnemy.transform.position -
                    transform.position;
            ShootBullet(w, direction, nearestEnemy);
        }
        
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

