using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    public float speedMult = 1f;

    public int damage = 10;

    private Vector3 direction;

    public Material materialSingle;
    public Material materialSpread;
    public Material materialFast;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;

        Debug.Log("Direction: " + direction);
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
        EnemyHealth enemy =
            other.GetComponent<EnemyHealth>();

        if (enemy != null)
            enemy.TakeDamage(damage);
        if(other.tag!="Player" && other.tag != "bullet")
            Destroy(gameObject);
    }
}