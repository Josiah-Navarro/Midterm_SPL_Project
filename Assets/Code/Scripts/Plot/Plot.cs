using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    private GameObject tower;
    private Color startColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (tower != null) return;
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > LevelManager.main.money)
        {
            Debug.Log("You can't afford this tower");
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        tower = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    }
}
