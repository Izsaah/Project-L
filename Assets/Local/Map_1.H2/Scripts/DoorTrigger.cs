using System.Collections;
using System.Collections.Generic;
using ProjectL.Scripts.Interface;
using UnityEngine;

namespace ProjectL.Local.Map_1_H2
{
    public class DoorTrigger : MonoBehaviour
    {
        public float openAngle = 90f;
        public float swingSpeed = 5f;

        private Quaternion closedRotation;
        private Quaternion openRotation;
        [SerializeField] private bool isOpen = false;

        // NEW: Stores the player's input script ONLY when they are standing near the door
        private IInputProvider playerInRange = null;

        void Start()
        {
            closedRotation = transform.rotation;
            openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
        }

        void Update()
        {
            // 1. Smoothly swing the door
            Quaternion targetRotation = isOpen ? openRotation : closedRotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * swingSpeed);

            // 2. Check for input every frame, but ONLY if the player is in range
            if (playerInRange != null && playerInRange.GetInteract())
            {
                ToggleDoor();
            }
        }

        public void ToggleDoor()
        {
            isOpen = !isOpen;
        }

        // Runs ONCE the exact moment the player steps into the trigger zone
        void OnTriggerEnter(Collider other)
        {
            IInputProvider input = other.GetComponent<IInputProvider>();
            if (input != null)
            {
                // Store the reference so Update() knows they are close enough
                playerInRange = input;
            }
        }

        // Runs ONCE the exact moment the player steps out of the trigger zone
        void OnTriggerExit(Collider other)
        {
            IInputProvider input = other.GetComponent<IInputProvider>();

            // If the thing leaving is the player, wipe the reference
            if (input != null && playerInRange == input)
            {
                playerInRange = null;
            }
        }
    }
}