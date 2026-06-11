using UnityEngine;
using UnityEngine.Audio;

public class EnemyHealth : MonoBehaviour
{
    public EnemyType enemyType;

    public int cost = 1;

    [Header("Base Stats")]
    public int baseHealth = 20;
    public int baseXPReward = 10;

    [HideInInspector]
    public int health;

    [HideInInspector]
    public int xpReward;

    public AudioMixerGroup sfxGroup;

    private bool isDead = false;

    public AudioClip audioDeath;

    void Awake()
    {
        health = baseHealth;
        xpReward = baseXPReward;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;
        isDead = true;

        //AudioSource.PlayClipAtPoint(audioDeath, gameObject.transform.position);
        PlaySFXAtPoint(audioDeath, gameObject.transform.position);

        GameManager.Instance.AddKill();

        GameObject player =
            GameObject.Find("Player");

        if (player != null)
        {
            player
                .GetComponent<PlayerXP>()
                .AddXP(xpReward);
        }

        Destroy(gameObject);
    }
    void PlaySFXAtPoint(AudioClip clip, Vector3 position)
    {
        GameObject tempGO = new GameObject("TempSFX");
        tempGO.transform.position = position;

        AudioSource aSource = tempGO.AddComponent<AudioSource>();

        aSource.clip = clip;
        aSource.outputAudioMixerGroup = sfxGroup;

        aSource.spatialBlend = 1f;
        aSource.rolloffMode = AudioRolloffMode.Logarithmic;

        aSource.Play();

        Destroy(tempGO, clip.length);
    }
}