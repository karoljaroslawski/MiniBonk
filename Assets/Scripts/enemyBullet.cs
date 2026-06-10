using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public int damage = 10;
    bool hit=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 2.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            PlayerHealth health = other.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                if (!hit) {  
                    hit = true; 
                    health.TakeDamage(10); 
                }
                Destroy(gameObject);
            }
        }
        else if (other.tag != "Enemy" && other.tag != "bullet")
            Destroy(gameObject);
    }
}
