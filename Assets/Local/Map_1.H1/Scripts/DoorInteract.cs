using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Player;
using ProjectL.Scripts.Interface;
using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    private Animator animator;

    private IInputProvider inputProvider;
    private bool isOpen = false;
    
    // THE SHIELD: Locks the door while it is moving
    private bool isAnimating = false; 

    // THE TRIGGER LOCK: Only lets the player interact if they are inside the box
    private bool isPlayerNear = false;

    [Header("Settings")]
    [Tooltip("The exact length of your folding animation in seconds")]
    public float animationLength = 1.0f; 

    void Start()
    {
        // Grabs the Animator component attached to the door
        animator = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            inputProvider = playerObj.GetComponent<IInputProvider>();
        }
        else
        {
            Debug.LogError("DoorInteract could not find the Player! Check your tags.");
        }
    }

    void Update()
    {
        // Now checks 3 things: Are they near? Did they press E? Is the door done moving?
        if (isPlayerNear && inputProvider.GetInteract() && !isAnimating)
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

    // --- NEW: TRIGGER ZONE LOGIC ---
    private void OnTriggerEnter(Collider other)
    {
        // When the player steps into the green box
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player steps out of the green box
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = false;
        }
    }
}
