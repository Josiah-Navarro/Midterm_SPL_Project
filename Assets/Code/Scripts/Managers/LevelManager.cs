using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int startingMoney;
    [SerializeField] private int startingHealth;
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;
    public int money;
    public int health;

    private void Awake()
    {
        main = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        money = startingMoney;
        health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void TakeDamage(int dmg)
    {
        if (health > 0) 
        {
            health -= dmg;
        } else
        {
            Debug.Log("No Health Left");
        }
    }
    public void IncreaseCurrency(int amount) 
    {
        money += amount;
    }
    public bool SpendCurrency(int amount) 
    {
        if (amount <= money)
        {
            money  -= amount;
            return true;
        } else 
        {
            Debug.Log("Not Enough Money");
            return false;
        }
    }
}
