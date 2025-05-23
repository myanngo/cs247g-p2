using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;
    [TextArea(2, 4)]
    [SerializeField] private string dialogue;

    public void Interact()
    {
        DialogueManager.Instance.ShowDialogue(npcName, dialogue);
        Debug.Log($"{npcName}: \"{dialogue}\"");
    }
}
