using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public RectTransform inventoryPanel;
    public GameObject inventoryItemPrefab;

    private void Awake()
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

    public void UpdateInventoryUI(List<TowerInventory.TowerEntry> inventory)
    {
        if (inventoryPanel == null || inventoryItemPrefab == null)
        {
            return;
        }

        foreach (Transform child in inventoryPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in inventory)
        {
            GameObject item = Instantiate(inventoryItemPrefab, inventoryPanel);
            
            TextMeshProUGUI textComponent = item.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"{entry.towerName} x{entry.count}";
            }

            Button button = item.GetComponent<Button>();
            if (button != null)
            {
                int towerID = entry.towerID;
                button.onClick.AddListener(() => BuildManager.Instance.SetSelectedTower(towerID));
            }
        }

    }
}
