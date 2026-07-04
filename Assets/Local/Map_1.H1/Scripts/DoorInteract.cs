using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;
    
    // THE SHIELD: Locks the door while it is moving
    private bool isAnimating = false; 

    [Header("Settings")]
    [Tooltip("The exact length of your folding animation in seconds")]
    public float animationLength = 1.0f; 

    void Start()
    {
        // Grabs the Animator component attached to the door
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Checks if the player presses E AND the door is NOT currently animating
        if (Input.GetKeyDown(KeyCode.E) && !isAnimating)
        {
            StartCoroutine(ToggleDoorRoutine());
        }
    }

    private IEnumerator ToggleDoorRoutine()
    {
        // 1. Lock the door so spamming E does nothing
        isAnimating = true; 
        
        isOpen = !isOpen; // Flips the boolean

        // 2. Play the correct animation
        if (isOpen)
        {
            animator.Play("DoorOpen");
        }
        else
        {
            animator.Play("DoorClose");
        }

        // 3. Force the script to wait until the animation is fully completed
        yield return new WaitForSeconds(animationLength);

        // 4. Unlock the door so the player can press E again!
        isAnimating = false; 
    }
}
