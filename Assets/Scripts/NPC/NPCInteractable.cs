using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;
    [TextArea(2, 4)]
    [SerializeField] private List<string> dialogue;
    [SerializeField] private Animator npcAnimator; // Optional animator component
    [SerializeField] private GameObject objectToActivate; // Optional object to appear after dialogue

    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(npcName, dialogue, npcAnimator, objectToActivate);
    }
}