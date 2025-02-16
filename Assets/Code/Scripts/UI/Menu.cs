using UnityEngine;
using TMPro;


public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isMenuOpen = true;
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
    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        anim.SetBool("MenuOpen", isMenuOpen);
    }
}
