using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
[CreateAssetMenu(fileName = "NewTowerData", menuName = "Tower Defense/Tower")]
public class TowerData : ScriptableObject
{
    public int towerID;
    public string towerName;
    public GameObject towerPrefab;
    public Image icon;
    public string rarity;
    public int cost;
    public bool isWaterTower;
}