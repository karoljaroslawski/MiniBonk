using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    List<EnemyHealth> hitEnemies = new List<EnemyHealth>();
    public GameObject sword;
    public List<EnemyHealth> GetEnemies(float scale)
    {
        hitEnemies.Clear();
        sword.GetComponent<Animator>().SetTrigger("PlaySwing");
        this.transform.localScale = Vector3.one * scale;
        BoxCollider boxCollider = this.GetComponent<BoxCollider>();

        Vector3 center = boxCollider.transform.TransformPoint(boxCollider.center);
        Vector3 halfExtents = (boxCollider.size / 2f) * scale;
        Quaternion rotation = boxCollider.transform.rotation;

        Collider[] collidersHit = Physics.OverlapBox(center, halfExtents, rotation);

        foreach (Collider c in collidersHit)
        {
            if (!c.CompareTag("Enemy"))
                continue;
            EnemyHealth enemy = c.GetComponentInParent<EnemyHealth>();
            if (enemy != null && !hitEnemies.Contains(enemy))
                hitEnemies.Add(enemy);
        }
        return hitEnemies;
    }

}
