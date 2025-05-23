using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;
    [TextArea(2, 4)]
    [SerializeField] private List<string> dialogueLines;

    public void Interact()
    {
         List<string> lines;

    if (InventoryManager.Instance.CountItem(ItemType.GlassPiece) >= 14)
    {
        lines = new List<string>
        {
            "Wow! Look at you, you little scavenger. Let’s see if we can piece something together with these …"
        };

        // Launch puzzle logic here too
    }
    else
    {
        lines = new List<string>
        {
            "We might as well make the best of a bad situation...",
            "Go collect all of the glass you can find — there should be 14 shards!"
        };
    }
        DialogueManager.Instance.ShowDialogue(npcName, dialogueLines);
    }
}
