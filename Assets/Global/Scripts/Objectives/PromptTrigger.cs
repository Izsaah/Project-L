using UnityEngine;
using ProjectL.Global.Scripts.UI;

namespace ProjectL.Global.Scripts.Objectives
{
    public enum InteractionType
    {
        PressE,
        HoldE
    }

    [RequireComponent(typeof(Collider))]
    public class PromptTrigger : MonoBehaviour
    {
        [Tooltip("Which UI should pop up when the player gets close?")]
        public InteractionType interactionType = InteractionType.PressE;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (InteractionPromptUI.Instance != null)
                {
                    if (interactionType == InteractionType.PressE)
                        InteractionPromptUI.Instance.ShowPressE();
                    else
                        InteractionPromptUI.Instance.ShowHoldE();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (InteractionPromptUI.Instance != null)
                {
                    InteractionPromptUI.Instance.HideAll();
                }
            }
        }
        
        void OnDisable()
        {
            if (InteractionPromptUI.Instance != null)
            {
                InteractionPromptUI.Instance.HideAll();
            }
        }
    }
}
