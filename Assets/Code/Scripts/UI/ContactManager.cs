using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class DialogueController : MonoBehaviour
{
    public string contactName;
    public TMP_Text cName;
    public MessageManager messageManager;
    private Queue<string> dialogueQueue = new Queue<string>();
    private Dictionary<string, string> responseMap = new Dictionary<string, string>();
    private bool awaitingEvent = false;
    private string waitingForEvent = "";
    private bool waiting = false;

    private List<string> codes = new List<string>();

    public Image icon;
    public Button button;

    private bool hasPendingReply = false;
    private string pendingReplyOptions = "";


    public void Setup(string name, MessageManager manager, Sprite icon)
    {
        messageManager = manager;
        changeName(name);
        this.icon.sprite = icon;
        button.onClick.AddListener(OnClick);
        Debug.Log("DialogueController: Setup complete for " + name);
    }

    public void changeName(string name)
    {
        contactName = name;
        cName.text = name;
        Debug.Log("DialogueController: Contact name set to " + name);
    }

    void OnClick()
    {
        Debug.Log("DialogueController: Clicked on " + contactName);
        messageManager.OpenChat(contactName);
    }

    public void LoadDialogue(List<string> lines)
    {
        dialogueQueue.Clear();
        responseMap.Clear();
        Debug.Log("DialogueController: Loading dialogue for " + contactName);

        foreach (string line in lines)
        {
            Debug.Log("DialogueController: Processing line - " + line);

            if (line.StartsWith("/Choose:"))
            {
                waiting = true;
                RequestReply(line.Replace("/Choose:", "").Trim());
            }
            else if (line.StartsWith(">")) // Handle new reply format
            {
                string[] parts = line.Substring(1).Split("->");
                if (parts.Length == 2)
                {
                    string option = parts[0].Trim();
                    string response = parts[1].Trim();
                    responseMap[option] = response;
                }
            }
            else if (line.StartsWith("/Await"))
            {
                awaitingEvent = true;

                waitingForEvent = line.Replace("/Await, ", "").Trim();
                Debug.Log("DialogueController: Awaiting event " + waitingForEvent);
            }
            else
            {
                dialogueQueue.Enqueue(line);
                Debug.Log("DialogueController: Enqueuing dialogue line: " + line);
            }
        }

        // **NEW**: Print all response mappings after loading
        Debug.Log("DialogueController: Final response mappings for " + contactName);
        foreach (var entry in responseMap)
        {
            Debug.Log("Option: '" + entry.Key + "' -> Response: '" + entry.Value + "'");
        }
    }

    public void RequestReply(string chooseLine)
    {
        Debug.Log("DialogueController: Showing reply immediately for " + contactName);
        messageManager.ShowReplyOptions(chooseLine);
        hasPendingReply = true;
        pendingReplyOptions = chooseLine;
    }


    public void StartDialogue()
    {
        Debug.Log("DialogueController: Starting dialogue for " + contactName);
        StartCoroutine(PlayDialogue());
    }

    private IEnumerator PlayDialogue()
    {
        while (dialogueQueue.Count > 0 && !waiting)
        {
            string line = dialogueQueue.Dequeue();
            Debug.Log("DialogueController: Processing line - " + line);

            if (line.StartsWith("/Await") && awaitingEvent)
            {
                Debug.Log("DialogueController: Waiting for event " + waitingForEvent);
                yield return new WaitUntil(() => !awaitingEvent);
            }
            else if (line.StartsWith("/Choose"))
            {
                waiting = true;
                RequestReply(line);
                Debug.Log("DialogueController: Showing reply panel");
            }
            else if (line.StartsWith(">"))
            {
                // Skip /Reply lines, since they are handled separately
                Debug.Log("DialogueController: Skipping Reply line");
            }
            else
            {
                messageManager.AddMessage(contactName, line.Trim('[', ']'), false);
                yield return new WaitForSeconds(1f);
            }
        }

        Debug.Log("DialogueController: Dialogue queue finished for " + contactName);
    }

    public void HandleReply(string chosenOption)
    {
        waiting = false;
        Debug.Log("DialogueController: Handling reply - '" + chosenOption + "'");

        // Print all stored responses before checking
        Debug.Log("DialogueController: Available responses: ");
        foreach (var entry in responseMap)
        {
            Debug.Log("Option: '" + entry.Key + "' -> Response: '" + entry.Value + "'");
        }
        if (responseMap.TryGetValue(chosenOption, out string npcResponse))
        {
            // Add NPC response
            messageManager.AddMessage(contactName, npcResponse, false);
            StartDialogue();
            hasPendingReply = false;
            pendingReplyOptions = "";
        }
        else
        {
            Debug.LogError("DialogueController: No response found for '" + chosenOption + "'");
        }
    }
    public void pending()
    {
        if (hasPendingReply)
            RequestReply(pendingReplyOptions);
        else
            Debug.Log("No pending");
    }
    public void TriggerEvent(string eventCode)
    {
        if (codes.Contains(eventCode))
        {
            codes.Remove(eventCode); // Remove the used event
            Debug.Log($"DialogueController: Event '{eventCode}' was in the list, resuming dialogue.");
            awaitingEvent = false;
            StartDialogue();
        }
        else if (awaitingEvent && waitingForEvent == eventCode)
        {
            awaitingEvent = false;
            Debug.Log($"DialogueController: Event triggered '{eventCode}', resuming dialogue.");
            StartDialogue();
        }
        else
        {
            Debug.Log($"DialogueController: Event '{eventCode}' stored for later.");
            codes.Add(eventCode); // Store event for future use
        }
    }

    internal void addEventCode(string eventCode)
    {
        if (!codes.Contains(eventCode))
        {
            codes.Add(eventCode);
            Debug.Log($"DialogueController: Added event code '{eventCode}'");

            // Check if this event can resume a waiting conversation
            if (awaitingEvent && waitingForEvent == eventCode)
            {
                codes.Remove(eventCode);
                awaitingEvent = false;
                Debug.Log($"DialogueController: Event '{eventCode}' matched waiting event, resuming dialogue.");
                StartDialogue();
            }
        }
    }
}

