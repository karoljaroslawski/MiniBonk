using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int kills = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void AddKill()
    {
        ++kills;

        UIManager.Instance.UpdateKills(
            kills
        );
    }
}