using UnityEngine;
using ProjectL.Global.Script.Player;

namespace ProjectL.Global.Script.CowRescue
{
    [RequireComponent(typeof(Collider))]
    public class RescueNPCInteract : MonoBehaviour
    {
        [Tooltip("The prompt text to show when looking at the NPC")]
        public string interactPrompt = "Press E to Carry NPC";
        
        private bool isPlayerInRange = false;
        private PlayerInputHandler playerInput;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                if (playerInput == null)
                {
                    playerInput = other.GetComponent<PlayerInputHandler>();
                }
                
                // Show prompt logic could go here if you have a UI for it
                // e.g. UIManager.Instance.ShowPrompt(interactPrompt);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                
                // Hide prompt logic could go here
                // e.g. UIManager.Instance.HidePrompt();
            }
        }

        private void Update()
        {
            if (isPlayerInRange && playerInput != null)
            {
                if (playerInput.GetInteract())
                {
                    // Call the manager to pick up the NPC
                    if (CowRescueManager.Instance != null)
                    {
                        CowRescueManager.Instance.PickUpInjuredNPC();
                    }
                }
            }
        }
    }
}
