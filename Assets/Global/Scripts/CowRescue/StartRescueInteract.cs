using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectL.Global.Script.Player;

namespace ProjectL.Global.Script.CowRescue
{
    [RequireComponent(typeof(Collider))]
    public class StartRescueInteract : MonoBehaviour
    {
        [Tooltip("The prompt text to show when looking at the fence/cow")]
        public string interactPrompt = "Press E to Pull Rope";

        [Header("Interact Prompt UI")]
        [Tooltip("Optional: Assign a World Space canvas TextMeshProUGUI to show above the trigger")]
        [SerializeField] private TextMeshProUGUI promptTextTMP;
        [Tooltip("Optional: Assign a Screen Space TextMeshProUGUI to show E prompt in HUD")]
        [SerializeField] private GameObject promptPanel;

        private bool isPlayerInRange = false;
        private PlayerInputHandler playerInput;

        private void Awake()
        {
            // Auto-find a prompt panel tagged InteractPrompt if not set
            // Wrapped in try-catch: tag must be registered in Tag Manager
            if (promptPanel == null)
            {
                try
                {
                    GameObject found = GameObject.FindWithTag("InteractPrompt");
                    if (found != null) promptPanel = found;
                }
                catch { /* Tag not registered — promptPanel stays null, use promptTextTMP instead */ }
            }
            CreateGroundZoneVisual();
            HidePrompt();
        }

        private void CreateGroundZoneVisual()
        {
            // 1. Create Ground Ring (Hollow Circle)
            GameObject groundZone = new GameObject("GroundZoneVisual");
            groundZone.transform.SetParent(this.transform, false);
            // Put it flat on the ground (assuming trigger center is Y=1.5, we offset by -1.4)
            groundZone.transform.localPosition = new Vector3(0, -1.4f, 0); 
            groundZone.transform.localRotation = Quaternion.Euler(90f, 0f, 0f); 

            LineRenderer lr = groundZone.AddComponent<LineRenderer>();
            lr.useWorldSpace = false;
            lr.loop = true;
            lr.startWidth = 0.15f;
            lr.endWidth = 0.15f;
            // Standard Unity unlit shader for simple white lines
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.white;
            lr.endColor = Color.white;
            
            int segments = 40;
            lr.positionCount = segments;
            float radius = 2.5f; // Radius matching typical trigger
            for (int i = 0; i < segments; i++)
            {
                float angle = (i * Mathf.PI * 2f) / segments;
                lr.SetPosition(i, new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0));
            }

            // (Removed Floating Downward Arrow as requested)
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = true;
                if (playerInput == null)
                    playerInput = other.GetComponent<PlayerInputHandler>();
                ShowPrompt();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerInRange = false;
                HidePrompt();
            }
        }

        private void Update()
        {
            if (isPlayerInRange && playerInput != null)
            {
                if (playerInput.GetInteract())
                {
                    if (CowRescueManager.Instance != null)
                    {
                        HidePrompt();
                        CowRescueManager.Instance.StartRopePullQTE();
                        // Disable this trigger so they can't restart it
                        gameObject.SetActive(false);
                    }
                }
            }
        }

        private void ShowPrompt()
        {
            if (promptTextTMP != null)
            {
                promptTextTMP.text = interactPrompt;
                promptTextTMP.gameObject.SetActive(true);
            }
            if (promptPanel != null) promptPanel.SetActive(true);
        }

        private void HidePrompt()
        {
            if (promptTextTMP != null) promptTextTMP.gameObject.SetActive(false);
            if (promptPanel != null) promptPanel.SetActive(false);
        }
    }
}
