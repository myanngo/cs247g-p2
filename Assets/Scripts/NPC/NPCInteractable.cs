using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;
    [TextArea(2, 4)]
    [SerializeField] private List<string> dialogue;

    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(npcName, dialogue);
    }
}
