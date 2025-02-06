using UnityEngine;
using TMPro;


public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.money.ToString();
    }

    public void SetSelected()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
