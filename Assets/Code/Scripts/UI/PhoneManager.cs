using UnityEngine;
using UnityEngine.UI;

public class PhoneManager : MonoBehaviour
{
    public GameObject phoneUI;
    public GameObject messagesScreen, purifierScreen, inboxScreen, gachaScreen;

    void Start()
    {
        phoneUI.SetActive(false); // Start with phone hidden
        ShowApp(null); // Hide all apps
    }

    public void TogglePhone()
    {
        phoneUI.SetActive(!phoneUI.activeSelf);
    }

    public void ShowApp(GameObject appScreen)
    {
        // Hide all screens
        messagesScreen.SetActive(false);
        purifierScreen.SetActive(false);
        inboxScreen.SetActive(false);
        gachaScreen.SetActive(false);

        // Show selected screen if any
        if (appScreen != null) appScreen.SetActive(true);
    }
}
