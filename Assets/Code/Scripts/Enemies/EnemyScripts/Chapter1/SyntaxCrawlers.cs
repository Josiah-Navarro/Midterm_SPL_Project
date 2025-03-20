using UnityEngine;

public class SyntaxCrawler : Enemy
{
    [Header("Syntax Crawler Attributes")]
    [SerializeField] private float healthMultiplier = 1.5f; 
    [SerializeField] private float speedMultiplier = 1.2f; 

    void Start()
    {
        hitpoints = Mathf.RoundToInt(hitpoints * healthMultiplier); 
        moveSpeed *= speedMultiplier;
    }
}
