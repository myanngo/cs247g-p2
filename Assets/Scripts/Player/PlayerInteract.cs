using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            float interactRange = 2f;
            Collider2D[] colliderArray = Physics2D.OverlapCircleAll(transform.position, interactRange);

            foreach (Collider2D c in colliderArray) {
                
                if (c.TryGetComponent(out NPCInteractable npcInteractable)) {
                    npcInteractable.Interact();
                }
            }
        }
    }
}
