using UnityEngine;
[CreateAssetMenu(fileName = "NewTower", menuName = "Tower Defense/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public string rank;
    public float attackRange;
    public float attackSpeed;
    public int damage;
    public int cost;
    public float rotationSpeed = 1000f;
    public GameObject bulletPrefab;
}