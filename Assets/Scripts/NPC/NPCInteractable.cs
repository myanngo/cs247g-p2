using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;

    [System.Serializable]
    public class StoryDialogue
    {
        public int stage;                     // Story stage this dialogue is for
        [TextArea(2, 4)]
        public List<string> lines;            // Lines to say
        public GameObject objectToActivate;   // Optional object to show at this stage
    }

    [SerializeField] private List<StoryDialogue> dialogueByStage;
    [SerializeField] private Animator npcAnimator; // Optional animator
    [TextArea(2, 4)]
    [SerializeField] private List<string> defaultLines;

    public void Interact()
    {
        int currentStage = Globals.StoryStage;

        foreach (var entry in dialogueByStage)
        {
            if (entry.stage == currentStage)
            {
                DialogueManager.Instance.ShowDialogue(
                    npcName,
                    entry.lines,
                    npcAnimator,
                    entry.objectToActivate
                );
                return;
            }
        }

        // Fallback if no matching stage
        DialogueManager.Instance.ShowDialogue(
            npcName,
            defaultLines,
            npcAnimator,
            null
        );
    }
}
