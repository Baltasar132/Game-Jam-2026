using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    /// Nullable
    IInteractable current;

    void FixedUpdate()
    {
        if (current != null && InputSystem.actions["Interact"].IsPressed())
        {
            current.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            current = interactable;
            current.ShowUI();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            current.HideUI();

            if (current == interactable)
            {
                current = null;
            }
        }
    }
}
