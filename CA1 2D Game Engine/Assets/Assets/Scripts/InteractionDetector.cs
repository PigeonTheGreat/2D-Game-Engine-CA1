using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; //For the OnInteract method

public class InteractionDetector : MonoBehaviour
{

    private IInteractible interactableInRange = null; //Closest Interactible
    public GameObject interactionIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractible interactible) && interactible.CanInteract())
        {
            interactableInRange = interactible;
            interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractible interactible) && interactible == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false);
        }
    }

}
