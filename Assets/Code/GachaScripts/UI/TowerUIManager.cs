using UnityEngine;
using UnityEngine.UI;

public class TowerUIManager : MonoBehaviour
{
    public static TowerUIManager Instance;

    [Header("UI Prefab Reference")]
    public GameObject towerOptionsPrefab; // Assign this in the Inspector

    private GameObject currentUIInstance;
    private Transform selectedTower;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTowerUI(Transform tower)
    {
        if (currentUIInstance != null)
        {
            Destroy(currentUIInstance); // Remove old UI if it exists
        }

        selectedTower = tower;

        // Instantiate the UI near the tower
        currentUIInstance = Instantiate(towerOptionsPrefab, tower.position, Quaternion.identity);
        currentUIInstance.transform.SetParent(GameObject.Find("Canvas").transform, false); // Attach to UI canvas
        currentUIInstance.transform.position = Camera.main.WorldToScreenPoint(tower.position + Vector3.up * 1.5f); // Position above the tower

        // Get buttons and assign functions dynamically
        Button upgradeButton = currentUIInstance.transform.Find("UpgradeButton").GetComponent<Button>();
        Button removeButton = currentUIInstance.transform.Find("RemoveButton").GetComponent<Button>();

        upgradeButton.onClick.AddListener(UpgradeTower);
        removeButton.onClick.AddListener(RemoveTower);
    }

    public void HideTowerUI()
    {
        if (currentUIInstance != null)
        {
            Destroy(currentUIInstance);
        }
        selectedTower = null;
    }

    private void UpgradeTower()
    {
        if (selectedTower != null)
        {
            BaseTower towerScript = selectedTower.GetComponent<BaseTower>();
            if (towerScript != null)
            {
                towerScript.UpgradeTower();
            }
        }

        HideTowerUI();
    }


    private void RemoveTower()
    {
        Debug.Log("Removing tower: " + selectedTower.name);
        Destroy(selectedTower.gameObject);
        HideTowerUI();
    }
}
