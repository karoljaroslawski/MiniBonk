using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance { get; private set; }

    public static float EnemySpeedMultiplier { get; private set; } = 1f;

    [Header("Visual Effects")]
    public GameObject battleFocusVfxPrefab;
    public GameObject arcaneShieldVfxPrefab;
    public GameObject timeWarpEnemyVfxPrefab;

    [Header("Audio")]
    public AudioSource audioSource;

    public AudioClip battleFocusSfx;
    public AudioClip arcaneShieldSfx;
    public AudioClip timeWarpSfx;

    [Header("References")]
    public PlayerHealth playerHealth;

    const float BattleFocusCooldown = 30f;
    const float BattleFocusDuration = 10f;
    const float BattleFocusBonus = 1.4f;

    const float ArcaneShieldCooldown = 45f;
    const float ArcaneShieldDuration = 5f;

    const float TimeWarpCooldown = 60f;
    const float TimeWarpDuration = 8f;
    const float TimeWarpEnemySpeed = 0.3f;

    float battleFocusCooldownTimer;
    float arcaneShieldCooldownTimer;
    float timeWarpCooldownTimer;

    float battleFocusActiveTimer;
    float arcaneShieldActiveTimer;
    float timeWarpActiveTimer;

    bool battleFocusActive;
    bool arcaneShieldActive;
    bool timeWarpActive;

    GameObject battleFocusVfxInstance;
    GameObject arcaneShieldVfxInstance;
    readonly List<TimeWarpVfxEntry> timeWarpEnemyVfx = new();

    class TimeWarpVfxEntry
    {
        public Transform enemy;
        public GameObject vfx;
    }

    public float MovementSpeedMultiplier =>
        battleFocusActive ? BattleFocusBonus : 1f;

    public float AttackSpeedMultiplier =>
        battleFocusActive ? BattleFocusBonus : 1f;

    public bool IsInvulnerable =>
        arcaneShieldActive;

    public float BattleFocusCooldownRemaining => battleFocusCooldownTimer;
    public float ArcaneShieldCooldownRemaining => arcaneShieldCooldownTimer;
    public float TimeWarpCooldownRemaining => timeWarpCooldownTimer;

    public bool IsBattleFocusActive => battleFocusActive;
    public bool IsArcaneShieldActive => arcaneShieldActive;
    public bool IsTimeWarpActive => timeWarpActive;

    void Awake()
    {
        Instance = this;

        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Update()
    {
        TickCooldowns();
        TickActiveAbilities();
        HandleInput();

        if (timeWarpActive)
            SyncTimeWarpEnemyVfx();
    }

    void TickCooldowns()
    {
        battleFocusCooldownTimer = Mathf.Max(0f, battleFocusCooldownTimer - Time.deltaTime);
        arcaneShieldCooldownTimer = Mathf.Max(0f, arcaneShieldCooldownTimer - Time.deltaTime);
        timeWarpCooldownTimer = Mathf.Max(0f, timeWarpCooldownTimer - Time.deltaTime);
    }

    void TickActiveAbilities()
    {
        if (battleFocusActive)
        {
            battleFocusActiveTimer -= Time.deltaTime;

            if (battleFocusActiveTimer <= 0f)
                EndBattleFocus();
        }

        if (arcaneShieldActive)
        {
            arcaneShieldActiveTimer -= Time.deltaTime;

            if (arcaneShieldActiveTimer <= 0f)
                EndArcaneShield();
        }

        if (timeWarpActive)
        {
            timeWarpActiveTimer -= Time.deltaTime;

            if (timeWarpActiveTimer <= 0f)
                EndTimeWarp();
        }
    }

    void HandleInput()
    {
        if (!CanUseAbilities() || Keyboard.current == null)
            return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
            TryActivateBattleFocus();

        if (Keyboard.current.eKey.wasPressedThisFrame)
            TryActivateArcaneShield();

        if (Keyboard.current.rKey.wasPressedThisFrame)
            TryActivateTimeWarp();
    }

    bool CanUseAbilities()
    {
        if (playerHealth != null && playerHealth.isDead)
            return false;

        if (UpgradeManager.Instance != null &&
            UpgradeManager.Instance.upgradeActive)
            return false;

        return Time.timeScale > 0f;
    }

    void PlayAbilitySound(AudioClip clip, float volume)
    {
        if (audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip, volume);
    }

    void TryActivateBattleFocus()
    {
        if (battleFocusActive || battleFocusCooldownTimer > 0f)
            return;

        battleFocusActive = true;
        battleFocusActiveTimer = BattleFocusDuration;
        battleFocusCooldownTimer = BattleFocusCooldown;

        SpawnPlayerVfx(battleFocusVfxPrefab, ref battleFocusVfxInstance);

        PlayAbilitySound(battleFocusSfx, 0.15f);
    }

    void EndBattleFocus()
    {
        battleFocusActive = false;
        battleFocusActiveTimer = 0f;

        ClearPlayerVfx(ref battleFocusVfxInstance);
    }

    void TryActivateArcaneShield()
    {
        if (arcaneShieldActive || arcaneShieldCooldownTimer > 0f)
            return;

        arcaneShieldActive = true;
        arcaneShieldActiveTimer = ArcaneShieldDuration;
        arcaneShieldCooldownTimer = ArcaneShieldCooldown;

        SpawnPlayerVfx(arcaneShieldVfxPrefab, ref arcaneShieldVfxInstance);

        PlayAbilitySound(arcaneShieldSfx, 0.15f);
    }

    void EndArcaneShield()
    {
        arcaneShieldActive = false;
        arcaneShieldActiveTimer = 0f;

        ClearPlayerVfx(ref arcaneShieldVfxInstance);
    }

    void TryActivateTimeWarp()
    {
        if (timeWarpActive || timeWarpCooldownTimer > 0f)
            return;

        timeWarpActive = true;
        timeWarpActiveTimer = TimeWarpDuration;
        timeWarpCooldownTimer = TimeWarpCooldown;

        EnemySpeedMultiplier = TimeWarpEnemySpeed;

        ApplyTimeWarpVfxToAllEnemies();

        PlayAbilitySound(timeWarpSfx, 0.25f);
    }

    void EndTimeWarp()
    {
        timeWarpActive = false;
        timeWarpActiveTimer = 0f;

        EnemySpeedMultiplier = 1f;

        ClearTimeWarpEnemyVfx();
    }

    void SpawnPlayerVfx(GameObject prefab, ref GameObject instance)
    {
        ClearPlayerVfx(ref instance);

        if (prefab == null)
            return;

        instance = Instantiate(prefab, transform);

        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
    }

    void ClearPlayerVfx(ref GameObject instance)
    {
        if (instance == null)
            return;

        Destroy(instance);
        instance = null;
    }

    void ApplyTimeWarpVfxToAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
            AttachTimeWarpVfx(enemy);
    }

    void SyncTimeWarpEnemyVfx()
    {
        for (int i = timeWarpEnemyVfx.Count - 1; i >= 0; i--)
        {
            if (timeWarpEnemyVfx[i].enemy == null)
            {
                if (timeWarpEnemyVfx[i].vfx != null)
                    Destroy(timeWarpEnemyVfx[i].vfx);

                timeWarpEnemyVfx.RemoveAt(i);
            }
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (!HasTimeWarpVfx(enemy.transform))
                AttachTimeWarpVfx(enemy);
        }
    }

    bool HasTimeWarpVfx(Transform enemy)
    {
        foreach (TimeWarpVfxEntry entry in timeWarpEnemyVfx)
        {
            if (entry.enemy == enemy)
                return true;
        }

        return false;
    }

    void AttachTimeWarpVfx(GameObject enemy)
    {
        if (enemy == null || timeWarpEnemyVfxPrefab == null)
            return;

        GameObject vfx =
            Instantiate(timeWarpEnemyVfxPrefab, enemy.transform);

        float scale = 1f;

        if (enemy.name.Contains("Enemy_Grunt"))
            scale = 2f;
        else if (enemy.name.Contains("Goblin"))
            scale = 3f;
        else if (enemy.name.Contains("Monster07_04"))
            scale = 4f;
        else if (enemy.name.Contains("LittleGhost_LP"))
            scale = 3f;
        else if (enemy.name.Contains("Sand Spider"))
            scale = 4f;

        vfx.transform.localScale = Vector3.one * scale;

        timeWarpEnemyVfx.Add(new TimeWarpVfxEntry
        {
            enemy = enemy.transform,
            vfx = vfx
        });
    }

    void ClearTimeWarpEnemyVfx()
    {
        foreach (TimeWarpVfxEntry entry in timeWarpEnemyVfx)
        {
            if (entry.vfx != null)
                Destroy(entry.vfx);
        }

        timeWarpEnemyVfx.Clear();
    }
}