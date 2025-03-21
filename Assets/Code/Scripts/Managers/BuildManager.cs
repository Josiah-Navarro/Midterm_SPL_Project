using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TowerData[] towerDataList;
    private Dictionary<int, TowerData> towerDictionary = new Dictionary<int, TowerData>();
    private int selectedTowerID = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple BuildManager instances found! Destroying extra instance.");
            Destroy(gameObject);
            return;
        }
        foreach (TowerData tower in towerDataList)
        {
            if (!towerDictionary.ContainsKey(tower.towerID))
            {
                towerDictionary.Add(tower.towerID, tower);
            }
            else
            {
                Debug.LogWarning($"Duplicate Tower ID found: {tower.towerID} for {tower.towerName}");
            }
        }
    }

    public TowerData GetSelectedTower()
    {
        if (selectedTowerID > -1 && towerDictionary.ContainsKey(selectedTowerID))
        {
            Debug.Log($"Tower with ID {selectedTowerID} found!");
            return towerDictionary[selectedTowerID];

        }
        return null;
    }

    public void SetSelectedTower(int id)
    {
        if (towerDictionary.ContainsKey(id))
        {
            selectedTowerID = id;
        }
        else
        {
            Debug.LogError($"Tower with ID {id} not found!");
        }
    }
}
