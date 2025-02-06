using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;
    private bool isDestroyed = false;
    public static int playerDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed) {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
    void Awake()
    {
        playerDamage = hitPoints;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static int GetPlayerDamage()
    {
        return playerDamage;
    }
}
