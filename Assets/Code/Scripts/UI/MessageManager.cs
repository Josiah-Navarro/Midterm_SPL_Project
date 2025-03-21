using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using System.Collections;
using Unity.VisualScripting;
using System;

public class MessageManager : MonoBehaviour
{
    public GameObject messagePrefab;
    public Transform chatPanel;
    public GameObject cPanel;
    public GameObject replyPanel;
    public GameObject replyButtonPrefab;
    private Dictionary<string, List<string>> chatHistory = new Dictionary<string, List<string>>();
    private string currentContact;
    private DialogueController activeDialogueController;

    void Start()
    {
        replyPanel.SetActive(false);
        Debug.Log("MessageManager: Initialized and reply panel hidden.");
    }
    public void CloseChat()
    {
        currentContact = "";
        cPanel.SetActive(false);
    }
    public void AddMessage(string contactName, string message, bool isPlayer)
    {
        Debug.Log("MessageManager: Adding message - " + message);
        if (!chatHistory.ContainsKey(contactName))
            chatHistory[contactName] = new List<string>();

        string formattedMessage = (isPlayer ? "Player: " : contactName + ": ") + message;
        chatHistory[contactName].Add(formattedMessage);

        // Force update UI after adding message
        if (currentContact != null)
            UpdateChatDisplay();
        else
            UpdateChatDisplay(contactName);
    }

    private void UpdateChatDisplay()
    {
        Debug.Log("MessageManager: Updating chat display for " + currentContact);

        foreach (Transform child in chatPanel)
        {
            // Prevent deletion of the scrollbar
            if (child.GetComponent<Scrollbar>() == null)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (string message in chatHistory[currentContact])
        {
            GameObject msg = Instantiate(messagePrefab, chatPanel);
            TMP_Text textComponent = msg.GetComponentInChildren<TMP_Text>();
            textComponent.text = message;

            // Change panel color based on sender
            Image panelImage = msg.GetComponent<Image>();  // Get the Panel's Image component
            if (panelImage != null)
            {
                if (!message.StartsWith("Player"))
                {
                    panelImage.color = Color.white;
                    textComponent.color = Color.gray;
                    textComponent.alignment = TextAlignmentOptions.Left;
                }
            }
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatPanel.GetComponent<RectTransform>());

        // Auto-scroll
        ScrollToBottom();
    }

    private void UpdateChatDisplay(string contact)
    {
        Debug.Log("MessageManager: Updating chat display for " + contact);

        foreach (Transform child in chatPanel)
        {
            // Prevent deletion of the scrollbar
            if (child.GetComponent<Scrollbar>() == null)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (string message in chatHistory[contact])
        {
            GameObject msg = Instantiate(messagePrefab, chatPanel);
            TMP_Text textComponent = msg.GetComponentInChildren<TMP_Text>();
            textComponent.text = message;

            // Change panel color based on sender
            Image panelImage = msg.GetComponent<Image>();  // Get the Panel's Image component
            if (panelImage != null)
            {
                if (!message.StartsWith("Player"))
                {
                    panelImage.color = Color.white;
                    textComponent.color = Color.gray;
                    textComponent.alignment = TextAlignmentOptions.Left;
                }
            }
        }
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatPanel.GetComponent<RectTransform>());

        // Auto-scroll
        ScrollToBottom();
    }


    public void OpenChat(string contactName)
    {
        Debug.Log("MessageManager: Opening chat for " + contactName);

        replyPanel.SetActive(false); // Hide any old replies

        currentContact = contactName;
        cPanel.SetActive(true);
        UpdateChatDisplay();

        // Find the active dialogue controller
        DialogueController activeDialogue = FindActiveDialogueController();

        // If this contact has a pending reply, show it now

        if (activeDialogue != null)
        {
            Debug.Log("MessageManager: Displaying pending reply for " + contactName);
            activeDialogue.pending();
        }
    }




    public void ShowReplyOptions(string chooseLine)
    {
        if (chooseLine == null)
            return;
        replyPanel.SetActive(true);
        foreach (Transform child in replyPanel.transform)
            Destroy(child.gameObject);

        // Extract choices from the new format
        string[] parts = chooseLine.Split('|');
        if (parts.Length < 2)
        {
            string option = parts[0].Trim();
            GameObject buttonObj = Instantiate(replyButtonPrefab, replyPanel.transform);
            buttonObj.GetComponentInChildren<TMP_Text>().text = option;
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectReply(option));
        }
        else
            for (int i = 1; i < parts.Length; i++)
            {
                string option = parts[i].Trim();
                GameObject buttonObj = Instantiate(replyButtonPrefab, replyPanel.transform);
                buttonObj.GetComponentInChildren<TMP_Text>().text = option;
                buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectReply(option));
            }
    }


    private DialogueController FindActiveDialogueController()
    {
        // Attempt to find an active DialogueController for the current contact
        foreach (DialogueController controller in FindObjectsOfType<DialogueController>())
        {
            if (controller.contactName == currentContact)
            {
                Debug.Log("MessageManager: Found active DialogueController for " + currentContact);
                return controller;
            }
        }

        Debug.LogError("MessageManager: No active dialogue controller found for " + currentContact);
        return null;
    }



    private void SelectReply(string chosenOption)
    {
        Debug.Log("MessageManager: Player selected reply - " + chosenOption);
        replyPanel.SetActive(false);
        activeDialogueController = FindActiveDialogueController();
        if (activeDialogueController != null)
        {
            // Add player's reply as a chat bubble
            AddMessage(currentContact, chosenOption, true);

            // Now pass the selected reply to the dialogue controller
            activeDialogueController.HandleReply(chosenOption);
        }
        else
        {
            Debug.LogError("MessageManager: No active dialogue controller for reply handling.");
        }
    }

    private void ScrollToBottom()
    {
        StartCoroutine(ScrollAfterFrame()); // Delayed scroll to ensure layout updates
    }

    private IEnumerator ScrollAfterFrame()
    {
        yield return null; // Wait for one frame to let UI update

        ScrollRect scrollRect = chatPanel.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();  // Forces UI to recalculate sizes
            scrollRect.verticalNormalizedPosition = 0f; // Moves to bottom
        }
    }

    internal string GetCurrentContact()
    {
        return currentContact;
    }
}
