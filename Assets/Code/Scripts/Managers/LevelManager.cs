using UnityEngine;
using System.Collections.Generic;
public class LevelManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int startingMoney;
    [SerializeField] private int startingHealth;
    [Header("Enemy Paths")]
    [SerializeField] public List<WaypointList> paths = new List<WaypointList>();

    public static LevelManager main;

    public Transform startPoint;
    public int money;
    public int health;
    public int corruptionPercentage;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {   
        money = startingMoney;
        health = startingHealth;
    }

    public List<Transform> GetPath(int pathIndex)
    {
        if (paths != null && pathIndex >= 0 && pathIndex < paths.Count)
        {
            return paths[pathIndex].waypoints;
        }
        return new List<Transform>(); 
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0) 
        {
            Debug.Log("No Health Left");
        }
    }

    public bool SpendCurrency(int amount) 
    {
        if (money >= amount)
        {
            money -= amount;
            return true;
        }
        Debug.Log("Not Enough Money");
        return false;
    }
    public void IncreaseCurrency(int amount)
    {
        money += amount;
        Debug.Log($"Currency increased by {amount}. Current money: {money}");
    }
}
[System.Serializable]
public class WaypointList
{
    public List<Transform> waypoints;
}