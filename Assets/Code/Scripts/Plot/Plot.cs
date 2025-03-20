using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private bool isWater;
    
    private GameObject tower;
    private Color startColor;

    void Awake()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>(); // Ensure SpriteRenderer is assigned
        }

        if (isWater)
        {
            startColor = Color.blue; // Water plots are blue
            hoverColor = new Color(0.5f, 0.7f, 1f); // Lighter blue for highlight
        }
        else
        {
            startColor = Color.grey; // Land plots start as grey
            hoverColor = Color.white; // Land plots highlight as white
        }

        if (sr != null)
            sr.color = startColor;
    }

    private void OnMouseEnter()
    {
        if (sr != null)
            sr.color = hoverColor; 
    }

    private void OnMouseExit()
    {
        if (sr != null)
            sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) {
            Debug.LogWarning("Cannot Be Placed Here");
            return;
        }

        TowerData towerToBuild = BuildManager.Instance.GetSelectedTower(); 
        if (towerToBuild == null)
        {
            Debug.LogWarning("No tower selected!");
            return;
        }

        if (towerToBuild.isWaterTower != isWater) 
        {
            Debug.Log("Cannot place this tower here!");
            return;
        }

        if (towerToBuild.cost > LevelManager.main.money)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        TowerInventory.TowerEntry entry = TowerInventory.Instance.inventory.Find(t => t.towerID == towerToBuild.towerID);
        if (entry == null || entry.count <= 0)
        {
            Debug.Log("No towers left in inventory!");
            return;
        }

        // Place the tower
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.towerPrefab, transform.position, Quaternion.identity); 

        // Remove tower from inventory
        TowerInventory.Instance.RemoveTowerFromUI(towerToBuild.towerName);
    }

}
