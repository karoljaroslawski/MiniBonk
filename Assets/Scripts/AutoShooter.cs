using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;

    private float timer;

    private WeaponManager wm;
    void Awake()
    {
        this.wm = new WeaponManager();
    }
    void Update()
    {
        timer += Time.deltaTime;
        foreach (Weapon w in this.wm.activeWeapons)
        {
            Shoot(w, timer);
        }
    }

    void ShootBullet(int damage, float speedMult, Vector3 direction, GameObject nearestEnemy)
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

        bulletScript.damage = damage;
        bulletScript.speedMult = speedMult;
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
                ShootBullet(w.damage, w.speedMult, rotation * direction, nearestEnemy);
            }
        }
        else
        {
            Vector3 direction =
                    nearestEnemy.transform.position -
                    transform.position;
            ShootBullet(w.damage, 1f, direction, nearestEnemy);
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

public class WeaponManager
{
    public Weapon pistol;
    public Weapon shotgun;
    public Weapon ar;
    public Weapon sniper;
    public List<Weapon> activeWeapons;

    public WeaponManager()
    {
        this.pistol = new Weapon(5, 1, 1f, 0.1f, 1, 1f, WeaponTypes.pistol);
        this.shotgun = new Weapon(3, 1, 2f, 0.1f, 5, 0.5f, WeaponTypes.shotgun);
        this.sniper = new Weapon(20, 1, 3f, 0.2f, 1, 2f, WeaponTypes.sniper);
        this.activeWeapons = new List<Weapon>();
        this.activeWeapons.Add(this.pistol);
    }

}

public class Weapon
{

    public int damage = 1;
    public int damageGain = 1;
    public float fireRate = 1;
    public float fireRateGain = 1;
    public int bulletNumber = 1;
    public float lastShot = 0;
    public float speedMult = 1f;
    public WeaponTypes weaponType = WeaponTypes.pistol;


    public Weapon(int damage, int damageGain, float fireRate, float fireRateGain, int bulletNumber, float speedMult, WeaponTypes weaponType)
    {
        this.damage = damage;
        this.damageGain = damageGain;
        this.fireRate = fireRate;
        this.fireRateGain = fireRateGain;
        this.bulletNumber = bulletNumber;
        this.weaponType = weaponType;
        this.speedMult = speedMult;
    }

    public void UpgradeWeapon()
    {
        this.damage += damageGain;
        this.fireRate -= fireRateGain;
    }
}

public enum WeaponTypes
{
    pistol, shotgun, ar, sniper
}