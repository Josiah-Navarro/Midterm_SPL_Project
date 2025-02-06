using UnityEngine;
using TMPro;

public class GameField : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI playerHealthUI;
    private void OnGUI()
    {
        playerHealthUI.text = LevelManager.main.health.ToString();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
