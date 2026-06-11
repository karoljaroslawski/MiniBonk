using System.Collections.Generic;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    public Weapon single;
    public Weapon shotgun;
    public Weapon sniper;
    public Weapon sword;
    public List<Weapon> activeWeapons;

    public WeaponManager()
    {
        this.single = new Weapon(8, 1, 1f, 0.1f, 1, 1f, WeaponTypes.single);
        this.shotgun = new Weapon(4, 1, 2.3f, 0.1f, 3, 0.3f, WeaponTypes.shotgun);
        this.sniper = new Weapon(20, 1, 4f, 0.2f, 1, 4f, WeaponTypes.sniper);
        this.sword = new Weapon(10, 2, 1f, 0.2f, 1, 4f, WeaponTypes.sword);
        this.activeWeapons = new List<Weapon>();
        this.activeWeapons.Add(this.single);
        //this.activeWeapons.Add(this.sword);
        //this.activeWeapons.Add(this.sniper);
    }

    public List<WeaponTypes> GetAviableUpgrades()
    {
        List<WeaponTypes> aviableUpgrades = new List<WeaponTypes>();
        if (this.single.level < 7) aviableUpgrades.Add(WeaponTypes.single);
        if (this.shotgun.level < 7) aviableUpgrades.Add(WeaponTypes.shotgun);
        if (this.sniper.level < 7) aviableUpgrades.Add(WeaponTypes.sniper);
        if (this.sword.level < 7) aviableUpgrades.Add(WeaponTypes.sword);

        return aviableUpgrades;
    }

    private Weapon getWeponByType(WeaponTypes wt)
    {
        switch (wt)
        {
            case WeaponTypes.single:
                return this.single;
            case WeaponTypes.shotgun:
                return this.shotgun;
            case WeaponTypes.sniper:
                return this.sniper;
            case WeaponTypes.sword:
                return this.sword;
        }
        return null;
    }

    public void UpgradeWeapon(WeaponTypes wt)
    {
        Weapon w = getWeponByType(wt);
        if (!activeWeapons.Contains(w)) activeWeapons.Add(w);
        else w.UpgradeWeapon();
    }

    public List<Weapon> GetActiveWeapons()
    {
        return activeWeapons;
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
    public int level = 0;
    public WeaponTypes weaponType = WeaponTypes.single;


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
        this.fireRate = Mathf.Max(this.fireRate - fireRateGain, 0.1f);

        if (this.weaponType == WeaponTypes.shotgun && level % 3 == 0)
            this.bulletNumber += 2;
        else if (this.weaponType == WeaponTypes.sword)
            this.bulletNumber = Mathf.Min(this.bulletNumber + 1, 4);
        level += 1;
    }
}