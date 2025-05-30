using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private Button continueButton;

    private Queue<string> dialogueLines = new Queue<string>();
    private bool isDialogueActive = false;

    public bool IsDialogueActive => isDialogueActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dialoguePanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }

        continueButton.onClick.AddListener(DisplayNextLine);
    }

    public void ShowDialogue(string npcName, List<string> lines)
    {
        isDialogueActive = true;
        dialoguePanel.SetActive(true);
        npcNameText.text = npcName;
        dialogueLines.Clear();

        foreach (string line in lines)
        {
            dialogueLines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueLines.Count == 0)
        {
            HideDialogue();
            return;
        }

        string nextLine = dialogueLines.Dequeue();
        dialogueText.text = nextLine;
    }

    public void HideDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
    }

    // Public method for PlayerInteract to call when E is pressed during dialogue
    public void HandleDialogueInput()
    {
        if (isDialogueActive)
        {
            DisplayNextLine();
        }
    }
}