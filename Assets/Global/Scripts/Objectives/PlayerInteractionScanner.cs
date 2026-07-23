using UnityEngine;

public class PlayerInteractionScanner : MonoBehaviour
{
    [Header("Scanner Settings")]
    [Tooltip("How far the player can reach to interact.")]
    public float interactDistance = 3f;
    
    [Tooltip("Which layers the raycast should hit. Set this to your 'Interactable' layer.")]
    public LayerMask interactableLayer; 

    private InteractableOutline currentInteractable;

    void Update()
    {
        // 1. Create a ray from the center of the screen (the camera) pointing forward
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // 2. Shoot the raycast. We only check objects that match the 'interactableLayer'
        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            // 3. We hit something! Check if it has our Outline script
            InteractableOutline interactable = hit.collider.GetComponent<InteractableOutline>();

            if (interactable != null)
            {
                // If we are looking at a NEW object, turn off the old one
                if (currentInteractable != null && currentInteractable != interactable)
                {
                    currentInteractable.OnLookAway();
                }

                // Turn on the outline for the new object
                currentInteractable = interactable;
                currentInteractable.OnLookAt();
            }
        }
        else
        {
            // 4. If the raycast hits nothing (we looked away), turn off the outline
            if (currentInteractable != null)
            {
                currentInteractable.OnLookAway();
                currentInteractable = null; // Clear the reference
            }
        }
    }
}
