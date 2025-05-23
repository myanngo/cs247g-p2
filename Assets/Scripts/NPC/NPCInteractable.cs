using UnityEngine;
using System.Collections.Generic;

public class NPCInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;

    private int dialogueStage = 0; // 0 = intro, 1 = glass shard mission, etc.

    public void Interact()
    {
        List<string> lines = new List<string>();

        if (dialogueStage == 0)
        {
            lines = new List<string>
            {
                "Excuse me, you spry, young thing. Could I ask for your help?",
                "The Great Tree, which is home to many of the forest’s creatures, is sick!",
                "All the pollutants have infected its roots and are spreading up its trunk.",
                "Luckily, I think I know a salve that will do the trick. But, I need your help collecting the materials. Would you help me and the forest?",
                "Thank you, sweetie. First, we’ll need some kind of bottle in which to make the elixir."
            };

            dialogueStage = 1; // advance to next stage
        }
        else if (dialogueStage == 1)
        {
            if (InventoryManager.Instance.CountItem(ItemType.GlassPiece) >= 14)
            {
                lines = new List<string>
                {
                    "We might as well make the best of a bad situation...",
                    "Go collect all of the glass you can find — there should be 14 shards!"
                };
            }
            else
            {
                lines = new List<string>
                {
                    "Wow! Look at you, you little scavenger.",
                    "Let’s see if we can piece something together with these …"
                };

                dialogueStage = 2;
                UnityEngine.SceneManagement.SceneManager.LoadScene("PuzzleInterface");
            }
        }
        else
        {
            lines = new List<string> { "Thanks again for your help, sweetie!" };
        }

        DialogueManager.Instance.ShowDialogue(npcName, lines);
    }
}
