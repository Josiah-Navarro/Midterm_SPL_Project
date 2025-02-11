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
        if (tower != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
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

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
