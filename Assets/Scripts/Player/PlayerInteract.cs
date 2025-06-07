using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Check if dialogue is already active
            if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
            {
                // If dialogue is active, progress to next line
                DialogueManager.Instance.HandleDialogueInput();
                return; // Don't check for new interactions
            }

            // If no dialogue is active, check for new interactions
            float interactRange = 2f;
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);

            foreach (Collider2D c in colliderArray) {
                
                if (c.TryGetComponent(out NPCInteractable npcInteractable)) {
                    npcInteractable.Interact();
                    break; // Only interact with the first NPC found
                }

                if (c.TryGetComponent(out BeaveressInteractable beaveressInteractable)) {
                    beaveressInteractable.Interact();
                    break; // Only interact with the first Beaveress found
                }
            }
        }
    }
}