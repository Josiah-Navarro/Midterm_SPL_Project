using UnityEngine;
[CreateAssetMenu(fileName = "NewTower", menuName = "Tower Defense/Tower")]
public class TowerData : ScriptableObject
{
    public Sprite icon;
    public string towerName;
    public string rarity;
    public float attackRange;
    public float attackSpeed;
    public int damage;
    public int cost;
    public float rotationSpeed = 1000f;
    public GameObject bulletPrefab;
}