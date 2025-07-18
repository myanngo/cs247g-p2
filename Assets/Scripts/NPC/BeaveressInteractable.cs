using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BeaveressInteractable : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private float sceneTransitionDelay = 3f;
    [SerializeField] private GameObject mixAnim;

    public void Interact()
    {
        List<string> lines = new List<string>();

        // INITIAL PHASE
        if (Globals.StoryStage == 0)
        {
            lines = new List<string>
            {
                "Excuse me, you spry, young thing. Could I ask for your help?",
                "The Great Tree, which is home to many of the forest's creatures, is sick!",
                "All the pollutants have infected its roots and are spreading up its trunk.",
                "Luckily, I think I know a salve that will do the trick. But, I need your help collecting the materials. Would you help me and the forest?",
                "Thank you, sweetie. First, we'll need some kind of bottle in which to make the elixir.",
                "The forest floor is covered with littered glass. We might as well make the best of a bad situation and piece some together to make a bottle for our salve.",
                "Go collect all of the glass you can find on the forest floor — there should be 14 shards!",
                "Come back if you need my assistance."
            };

            Globals.StoryStage = 1;
        }
        // GLASS PUZZLE FINDING PHASE
        else if (Globals.StoryStage == 1)
        {
            if (InventoryData.Instance.CountItem(ItemType.GlassPiece) < 14)
            {
                lines = new List<string>
                {
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

                Globals.StoryStage = 2;

                // Use FadeManager for scene transition
                if (FadeManager.Instance != null)
                {
                    FadeManager.Instance.FadeToSceneWithDelay("PuzzleInterface", sceneTransitionDelay);
                }
                else
                {
                    Debug.LogWarning("FadeManager not found! Using direct scene load.");
                    StartCoroutine(DelayedSceneLoad("PuzzleInterface", sceneTransitionDelay));
                }
            }
        }
        // POST PUZZLE PHASE (GO FILL UP)
        else if (Globals.StoryStage == 2)
        {
            lines = new List<string>
            {
                "Wahoo! Way to go pumpkin!",
                "Now, we’ll need water as a base for our salve. There’s a spring just East of here where my slightly ill-mannered friends hangs out."
            };

            if (InventoryData.Instance.CountItem(ItemType.FilledBottle) > 0)
            {
                Globals.StoryStage = 3;
            }
        }
        // FLOWER PUZZLE PHASE
        else if (Globals.StoryStage == 3)
        {
            lines = new List<string>
            {
                "Now, we need nectar from the lily of the valley, for tranquillity -- to put the Great Tree at ease.",
                "I've heard the pasture at the Northeast of the forest has a few of the rare flower!"
            };

            if (InventoryData.Instance.CountItem(ItemType.LilyofValley) > 0)
            {
                Globals.StoryStage = 4;
            }
        }
        // GET HONEY PHASE
        else if (Globals.StoryStage == 4)
        {
            lines = new List<string>
            {
                "Finally, we need to give this old tree a little something sweet and punchy — we need a bit of fresh honey, for verve! There must be someone around with a sweet tooth…"
            };
        }
        // FINAL PHASE
        else if (Globals.StoryStage == 5)
        {
            lines = new List<string>
            {
                "My goodness! You did it!",
                "Let’s see –",
            };
            StartCoroutine(PlayMixAnimationAndTransition(sceneTransitionDelay));
        }
        // DEFAULT
        else
        {
            lines = new List<string> { "Thanks again for your help, sweetie!" };
        }

        DialogueManager.Instance.ShowDialogue(npcName, lines);
    }

    private System.Collections.IEnumerator DelayedSceneLoad(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private IEnumerator PlayMixAnimationAndTransition(float delay)
    {
        yield return new WaitForSeconds(delay);

        mixAnim.SetActive(true);
        Animator animator = mixAnim.GetComponent<Animator>();
        yield return new WaitForSeconds(delay + 3f);

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeToScene("FinalAnimation");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("FinalAnimation");
        }
    }
}
