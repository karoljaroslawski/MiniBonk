using System.Collections.Generic;
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject magicMissilePrefab;
    public GameObject fireSurgePrefab;
    public GameObject sparkBoltPrefab;

    private float timer;

    public MeleeWeapon meleeWeapon;

    public WeaponManager wm;

    public AudioSource audioSource;
    public AudioClip audioShoot;
    public AudioClip swordSound;
    private float lastSwordSoundTime;

    void Update()
    {
        timer += Time.deltaTime;
        foreach (Weapon w in this.wm.activeWeapons)
        {
            Shoot(w, timer);
        }
    }

    GameObject GetBulletPrefab(WeaponTypes weaponType)
    {
        switch (weaponType)
        {
            case WeaponTypes.shotgun:
                return fireSurgePrefab;

            case WeaponTypes.sniper:
                return sparkBoltPrefab;

            default:
                return magicMissilePrefab;
        }
    }

    void ShootBullet(Weapon w, Vector3 direction, GameObject nearestEnemy)
    {
        GameObject bullet =
        Instantiate(
            GetBulletPrefab(w.weaponType),
            transform.position + Vector3.up,
            Quaternion.identity
        );

        Bullet bulletScript =
            bullet.GetComponent<Bullet>();

        bulletScript.SetDirection(direction);

        bulletScript.damage = w.damage;
        bulletScript.speedMult = w.speedMult;
        bulletScript.maxEnemyHits = GetMaxEnemyHits(w);
    }

    int GetMaxEnemyHits(Weapon w)
    {
        switch (w.weaponType)
        {
            case WeaponTypes.shotgun:
                return 5 + w.level;
            case WeaponTypes.sniper:
                return 20 + w.level * 2;
            default:
                return 1;
        }
    }

    void Shoot(Weapon w, float timer)
    {
        float attackSpeedMultiplier = 1f;
        if (AbilityManager.Instance != null)
            attackSpeedMultiplier = AbilityManager.Instance.AttackSpeedMultiplier;

        float effectiveFireRate = w.fireRate / attackSpeedMultiplier;

        if (timer - w.lastShot < effectiveFireRate)
            return;

        w.lastShot = timer;
        GameObject nearestEnemy = FindNearestEnemy();

        if (nearestEnemy == null)
            return;

        if (w.weaponType == WeaponTypes.sword)
        {
            if (Time.time - lastSwordSoundTime > 0.4f)
            {
                audioSource.PlayOneShot(swordSound, 0.15f);
                lastSwordSoundTime = Time.time;
            }
        }
        else
        {
            audioSource.PlayOneShot(audioShoot, 0.5f);
        }

        if (w.weaponType == WeaponTypes.shotgun) {
            for (int i = 0; i < w.bulletNumber; i++)
            {
                Vector3 direction = nearestEnemy.transform.position - transform.position;
                float spread = 45f/w.bulletNumber;
                Quaternion rotation = Quaternion.AngleAxis((i-w.bulletNumber/2)*spread, Vector3.up);
                ShootBullet(w, rotation * direction, nearestEnemy);
            }
        }
        else if (w.weaponType == WeaponTypes.sword)
        {
            List<EnemyHealth> enemyList = this.meleeWeapon.GetEnemies(((float)w.bulletNumber/2f)+1f);
            foreach (EnemyHealth enemy in enemyList)
            {
                enemy.TakeDamage(w.damage);
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

