using UnityEngine;

// This ensures that when you add this script, it automatically adds an Outline component
[RequireComponent(typeof(Outline))] 
public class InteractableOutline : MonoBehaviour
{
    private Outline _outline;

    void Start()
    {
        // Get the outline component and configure it
        _outline = GetComponent<Outline>();
        
        _outline.OutlineMode = Outline.Mode.OutlineAll;
        _outline.OutlineColor = Color.yellow; // Choose your color
        _outline.OutlineWidth = 4f;           // Choose the thickness
        
        // Turn it off by default
        _outline.enabled = false;
    }

    // Called by the PlayerInteractionScanner when the crosshair is on the object
    public void OnLookAt() 
    {
        _outline.enabled = true;
    }
    
    // Called when the crosshair leaves the object
    public void OnLookAway()
    {
        _outline.enabled = false;
    }
}
