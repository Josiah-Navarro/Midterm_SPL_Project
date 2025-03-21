using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class DialogueLoader : MonoBehaviour
{
    public List<ContactData> contacts; // List of contacts
    public GameObject contactPrefab;
    public Transform contactListParent;
    public MessageManager messageManager;

    private Dictionary<string, DialogueController> contactDialogues = new Dictionary<string, DialogueController>();

    void Start()
    {
        foreach (ContactData contact in contacts)
        {
            CreateContact(contact);
            StartConversation(contact.contactName);
        }
    }

    void CreateContact(ContactData contactData)
    {
        Debug.Log("DialogueLoader: Instantiating contact " + contactData.contactName);

        GameObject contactObj = Instantiate(contactPrefab, contactListParent);
        DialogueController controller = contactObj.GetComponent<DialogueController>();

        if (controller == null)
        {
            Debug.LogError("DialogueLoader: Missing DialogueController in prefab!");
            return;
        }

        // Set up contact UI
        controller.Setup(contactData.contactName, messageManager, contactData.contactIcon);

        // Load dialogue from text file
        List<string> dialogueLines = new List<string>(contactData.dialogueFile.text.Split('\n'));
        controller.LoadDialogue(dialogueLines);

        contactDialogues[contactData.contactName] = controller;
        Debug.Log("DialogueLoader: Successfully created contact " + contactData.contactName);
    }

    public void TriggerContactEvent(string eventCode)
    {
        foreach (ContactData contact in contacts)
        {
            if (contact.eventCode == eventCode && !contactDialogues.ContainsKey(contact.contactName))
            {
                Debug.Log("DialogueLoader: Unlocking contact " + contact.contactName);
                CreateContact(contact);
                StartConversation(contact.contactName);
                return;
            }
        }
        Debug.LogError("DialogueLoader: No contact found for event " + eventCode);
    }

    public void TriggerEvent(string contactName, string eventCode)
    {
        if (contactDialogues.TryGetValue(contactName, out DialogueController controller))
        {
            controller.addEventCode(eventCode); // Add the event code to the list
            Debug.Log($"DialogueLoader: Added event code '{eventCode}' to {contactName}");
        }
        else
        {
            Debug.LogError($"DialogueLoader: Contact '{contactName}' not found in active dialogues!");
        }
    }


    public void StartConversation(string contactName)
    {
        Debug.Log("DialogueLoader: Attempting to start conversation with " + contactName);
        if (contactDialogues.ContainsKey(contactName))
        {
            contactDialogues[contactName].StartDialogue();
        }
        else
        {
            Debug.LogError("DialogueLoader: Contact not found - " + contactName);
        }
    }
}