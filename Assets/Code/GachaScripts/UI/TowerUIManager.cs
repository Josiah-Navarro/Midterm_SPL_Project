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

    public void ShowTowerUI(BaseTower tower)
    {
        if (currentUIInstance != null)
        {
            Destroy(currentUIInstance);
        }

        selectedTower = tower.transform; // Store transform separately

        // Instantiate the UI near the tower
        currentUIInstance = Instantiate(towerOptionsPrefab);
        currentUIInstance.transform.SetParent(GameObject.Find("Canvas").transform, false);
        currentUIInstance.transform.position = Camera.main.WorldToScreenPoint(tower.transform.position);

        // Get buttons and assign functions dynamically
        Button upgradeButton = currentUIInstance.transform.Find("UpgradeButton").GetComponent<Button>();
        Button removeButton = currentUIInstance.transform.Find("RemoveButton").GetComponent<Button>();

        upgradeButton.onClick.AddListener(() => UpgradeTower(tower));
        removeButton.onClick.AddListener(() => RemoveTower(tower));
    }

    public void HideTowerUI()
    {
        if (currentUIInstance != null)
        {
            Destroy(currentUIInstance);
        }
        selectedTower = null;
    }

    private void UpgradeTower(BaseTower tower)
    {
        if (tower != null)
        {
            tower.UpgradeTower();
        }

        HideTowerUI();
    }


    private void RemoveTower(BaseTower tower)
    {
        Debug.Log("Removing tower: " + tower.name);
        Destroy(tower.gameObject);
        HideTowerUI();
    }
}
