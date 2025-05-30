using UnityEngine;
using System.Collections.Generic;

public class BeaveressInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private float sceneTransitionDelay = 3f; // Delay before scene transition

    private int dialogueStage = 0; // 0 = intro, 1 = glass shard mission, etc.

    public void Interact()
    {
        List<string> lines = new List<string>();

        if (dialogueStage == 0)
        {
            lines = new List<string>
            {
                "Excuse me, you spry, young thing. Could I ask for your help?",
                "The Great Tree, which is home to many of the forest's creatures, is sick!",
                "All the pollutants have infected its roots and are spreading up its trunk.",
                "Luckily, I think I know a salve that will do the trick. But, I need your help collecting the materials. Would you help me and the forest?",
                "Thank you, sweetie. First, we'll need some kind of bottle in which to make the elixir. Come back if you need my assistance."
            };

            dialogueStage = 1; // advance to next stage
        }
        else if (dialogueStage == 1)
        {
            if (InventoryManager.Instance.CountItem(ItemType.GlassPiece) < 14)
            {
                lines = new List<string>
                {
                    "The forest floor is covered with littered glass. We might as well make the best of a bad situation and piece some together to make a bottle for our salve.",
                    "Go collect all of the glass you can find on the forest floor — there should be 14 shards!"
                };
            }
            else
            {
                lines = new List<string>
                {
                    "Wow! Look at you, you little scavenger.",
                    "Let's see if we can piece something together with these …"
                };

                dialogueStage = 2;
                
                // Use FadeManager for scene transition with delay
                if (FadeManager.Instance != null)
                {
                    FadeManager.Instance.FadeToSceneWithDelay("PuzzleInterface", sceneTransitionDelay);
                }
                else
                {
                    // Fallback if FadeManager is not available
                    Debug.LogWarning("FadeManager not found! Using direct scene load.");
                    StartCoroutine(DelayedSceneLoad("PuzzleInterface", sceneTransitionDelay));
                }
            }
        }
        else
        {
            lines = new List<string> { "Thanks again for your help, sweetie!" };
        }

        DialogueManager.Instance.ShowDialogue(npcName, lines);
    }

    // Fallback coroutine for delayed scene loading without fade
    private System.Collections.IEnumerator DelayedSceneLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}