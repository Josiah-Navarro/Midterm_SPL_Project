using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    private List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void UnRegisterEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public List<Enemy> GetEnemiesInRange(Vector3 position, float range)
    {
        List<Enemy> enemiesInRange = new List<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(position, enemy.transform.position) <= range)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    public List<Enemy> GetAllEnemies()
    {
        return enemies;
    }
}
