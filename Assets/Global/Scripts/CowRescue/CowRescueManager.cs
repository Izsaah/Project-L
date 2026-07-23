using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ProjectL.Global.Script.Movement;
using ProjectL.Global.Script.Camera;
using ProjectL.Global.Script.Inventory;

namespace ProjectL.Global.Script.CowRescue
{
    public class CowRescueManager : MonoBehaviour
    {
        public static CowRescueManager Instance { get; private set; }

        [Header("Player References")]
        [SerializeField] private GameObject playerObject;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private FirstPersonCamera playerCamera;
        [SerializeField] private PlayerInventory playerInventory;

        [Header("NPC & Cow References")]
        [SerializeField] private NPCController npcController;
        [SerializeField] private Animator npcAnimator;
        [SerializeField] private Animator cowAnimator;
        [SerializeField] private Transform chuongBoDestination;

        [Header("QTE Minigame References")]
        [SerializeField] private PullRopeMinigame qteMinigame;
        [SerializeField] private GameObject ropeVisual;

        [Header("Flood & Siren Settings")]
        [SerializeField] private GameObject floodWaterObject;
        [SerializeField] private AudioSource sirenAudioSource;
        
        [Header("Injured NPC / Rescue System")]
        [SerializeField] private GameObject npcNormalVisual;
        [SerializeField] private GameObject npcInjuredGroundVisual;
        [SerializeField] private GameObject carryTriggerObject;
        [SerializeField] private ItemData injuredNpcItemData; // Item with size = IS.h (Heavy)

        [Header("Events")]
        public UnityEvent onRescueSequenceStart;
        public UnityEvent onRescueWin;
        public UnityEvent onRescueLose; // Triggers flood
        public UnityEvent onGameCompleted; // When reaching safe zone with NPC

        private bool isCarryingNPC = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (npcInjuredGroundVisual != null && npcInjuredGroundVisual != npcNormalVisual) npcInjuredGroundVisual.SetActive(false);
            if (carryTriggerObject != null) carryTriggerObject.SetActive(false);
            if (ropeVisual != null) ropeVisual.SetActive(false);
        }

        private void Start()
        {
            // Auto-find player references if not assigned
            if (playerObject == null) playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                if (playerMovement == null) playerMovement = playerObject.GetComponent<PlayerMovement>();
                if (playerCamera == null) playerCamera = playerObject.GetComponentInChildren<FirstPersonCamera>();
                if (playerInventory == null) playerInventory = playerObject.GetComponent<PlayerInventory>();
            }
        }

        // --- STEP 1: NPC walks to Chuong Bo ---
        public void StartNPCWalkToChuongBo()
        {
            if (npcController != null && chuongBoDestination != null)
            {
                npcController.WalkTo(chuongBoDestination, () => {
                    Debug.Log("NPC has arrived at the cow shed. Ready for pull interaction.");
                    // Optional: automatically trigger dialogue or action prompt
                });
            }
        }

        // --- STEP 2: Start Rope Pull QTE ---
        public void StartRopePullQTE()
        {
            if (qteMinigame == null)
            {
                Debug.LogError("QTE Minigame reference is missing!");
                return;
            }

            // Lock Player
            SetPlayerControls(false);

            // Turn on pulling visuals
            if (ropeVisual != null) ropeVisual.SetActive(true);

            // Trigger anims
            if (npcAnimator != null) npcAnimator.SetBool("isPulling", true);
            if (cowAnimator != null) cowAnimator.SetBool("isStuck", true);

            // Start minigame
            qteMinigame.StartMinigame(OnQTEFinished);
            onRescueSequenceStart?.Invoke();
        }

        private void OnQTEFinished(bool isSuccess)
        {
            // Turn off pulling visuals
            if (ropeVisual != null) ropeVisual.SetActive(false);
            if (npcAnimator != null) npcAnimator.SetBool("isPulling", false);

            if (isSuccess)
            {
                HandleWin();
            }
            else
            {
                HandleLose();
            }
        }

        // --- BRANCH A: QTE SUCCESS (Win) ---
        private void HandleWin()
        {
            Debug.Log("Cow rescue SUCCESSFUL!");

            // Trigger animations
            if (cowAnimator != null)
            {
                cowAnimator.SetBool("isStuck", false);
                cowAnimator.SetBool("isWalking", true);
            }

            // Make NPC and Cow walk away to safety (Timeline / Scripted walk)
            // For simplicity, we can let NPC walk to a safe point
            if (npcController != null && chuongBoDestination != null)
            {
                // Walk to safe zone
                npcController.WalkTo(chuongBoDestination, () => {
                    // Completed
                });
            }

            // Unlock Player
            SetPlayerControls(true);

            onRescueWin?.Invoke();
        }

        // --- BRANCH B: QTE FAIL (Lose - Flood) ---
        private void HandleLose()
        {
            Debug.Log("Cow rescue FAILED! Water is rising!");

            // Turn on flood mechanics
            if (floodWaterObject != null) floodWaterObject.SetActive(true);
            if (sirenAudioSource != null) sirenAudioSource.Play();

            // Setup NPC Injured state
            if (npcNormalVisual != null) npcNormalVisual.SetActive(false);
            if (npcInjuredGroundVisual != null) npcInjuredGroundVisual.SetActive(true);
            if (carryTriggerObject != null) carryTriggerObject.SetActive(true);

            // Unlock Player to run and rescue the NPC
            SetPlayerControls(true);

            onRescueLose?.Invoke();
        }

        // --- STEP 3: Carrying/Bế người đàn ông ---
        public void PickUpInjuredNPC()
        {
            if (isCarryingNPC) return;

            if (playerInventory != null && injuredNpcItemData != null)
            {
                // Add the Heavy item representing the NPC to player's inventory
                // This triggers the heavy weight restriction (slowing down movement) automatically!
                bool added = playerInventory.AddItem(injuredNpcItemData);
                if (added)
                {
                    isCarryingNPC = true;
                    
                    // Hide ground NPC visual
                    if (npcInjuredGroundVisual != null) npcInjuredGroundVisual.SetActive(false);
                    if (carryTriggerObject != null) carryTriggerObject.SetActive(false);

                    Debug.Log("Player is now carrying the NPC! Move slowly to the Safe Zone.");
                }
                else
                {
                    Debug.LogWarning("Inventory full! Make sure you drop other items to carry the NPC.");
                    // You can trigger a monologue here: "My hands are full, I need to drop something!"
                    if (Core.GameMaster.GameManager.Instance != null && Core.GameMaster.GameManager.Instance.monologueUI != null)
                    {
                        Core.GameMaster.GameManager.Instance.monologueUI.ST("heavy_item"); 
                    }
                }
            }
        }

        // --- STEP 4: Arrived at Safe Zone ---
        public void PlayerArrivedAtSafeZone()
        {
            // Verify if player is carrying the NPC
            if (isCarryingNPC)
            {
                Debug.Log("Rescue Mission COMPLETE! Safe Zone reached.");
                
                // Remove NPC from inventory
                if (playerInventory != null && injuredNpcItemData != null)
                {
                    playerInventory.RemoveItem(injuredNpcItemData);
                }

                isCarryingNPC = false;
                onGameCompleted?.Invoke();
            }
            else
            {
                Debug.Log("Safe zone reached, but the NPC is still left behind!");
            }
        }

        private void SetPlayerControls(bool state)
        {
            if (playerMovement != null) playerMovement.enabled = state;
            if (playerCamera != null) playerCamera.enabled = state;
            
            // Re-lock or release cursor
            if (state)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
