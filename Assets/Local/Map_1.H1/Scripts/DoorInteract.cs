using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Player;
using ProjectL.Scripts.Interface;
using UnityEngine;

namespace ProjectL.Local.Map_1_H1
{
    public class DoorInteract : MonoBehaviour
    {
    private Animator animator;

    private IInputProvider inputProvider;
    private bool isOpen = false;
    private bool isAnimating = false; 

    [Header("Lock Settings")]
    public bool isLocked = true; // Starts locked
    private bool isPlayerNear = false;

    [Header("Settings")]
    [Tooltip("The exact length of your folding animation in seconds")]
    public float animationLength = 1.0f; 

    void Start()
    {
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
        // FIXED: Added !isLocked condition
        if (!isLocked && isPlayerNear && inputProvider != null && inputProvider.GetInteract() && !isAnimating)
        {
            StartCoroutine(ToggleDoorRoutine());
        }
    }

    private IEnumerator ToggleDoorRoutine()
    {
        isAnimating = true; 
        isOpen = !isOpen; 

        if (isOpen)
        {
            animator.Play("DoorOpen");
        }
        else
        {
            animator.Play("DoorClose");
        }

        yield return new WaitForSeconds(animationLength);
        isAnimating = false; 
    }

    public void UnlockDoor()
    {
        isLocked = false;
        Debug.Log("The door has been unlocked!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            isPlayerNear = false;
        }
    }
}
}