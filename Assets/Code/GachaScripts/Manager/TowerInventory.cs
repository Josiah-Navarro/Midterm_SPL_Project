using System.Collections.Generic;
using UnityEngine;

public class TowerInventory : MonoBehaviour
{
    public static TowerInventory Instance;
    
    [System.Serializable]
    public class TowerEntry
    {
        public int towerID;
        public string towerName;
        public GameObject towerPrefab;
        public int count;
    }
    
    public List<TowerEntry> inventory = new List<TowerEntry>();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddTower(TowerData tower, int amount = 1)
    {
        string towerName = tower.towerName;
        TowerEntry existingEntry = inventory.Find(t => t.towerName == towerName);
        
        if (existingEntry != null)
        {
            existingEntry.count += amount;
        }
        else
        {
            TowerEntry newEntry = new TowerEntry { towerID = tower.towerID, towerName = towerName, towerPrefab = tower.towerPrefab, count = amount };
            inventory.Add(newEntry);
        }

        InventoryUI.Instance.UpdateInventoryUI(inventory);
    }

    public void RemoveTowerFromUI(string towerName)
    {
        TowerEntry entry = inventory.Find(t => t.towerName == towerName);
        if (entry != null && entry.count > 0)
        {
            // Reduce count in inventory
            entry.count--;

            // Remove the entry if count reaches 0
            if (entry.count == 0)
            {
                inventory.Remove(entry);
            }

            // Update UI
            InventoryUI.Instance.UpdateInventoryUI(inventory);  
        }
    }


    public int GetTowerCount(string towerName)
    {
        TowerEntry entry = inventory.Find(t => t.towerName == towerName);
        return entry != null ? entry.count : 0;
    }
    public void AddSummonedTowers(List<TowerData> pulledTowers)
    {
        foreach (TowerData tower in pulledTowers)
        {
            AddTower(tower, 1);
        }
    }
}
