using UnityEngine;
[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower")]
public class TowerData : ScriptableObject
{
    public int towerID;
    public string towerName;
    public GameObject towerPrefab;
    public string rarity;
    public int cost;
    public bool isWaterTower;
}