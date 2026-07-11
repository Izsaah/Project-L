using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.Inventory;
using ProjectL.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ProjectL.Global.Script.interaction
{
    public class RequiredItemHoldInteraction : MonoBehaviour
    {
        [Header("HOW LONG HAVE IT BEEN HAHAHAHAHAH")]
        public int timeCostInMinutes = 30;

        [Header("YOOO WHAT IN YO TOOL BOX DUMBASS!!!!")]
        public List<ItemData> requiredTool = new List<ItemData>();
        public String actionText = "";

        [Header("my record is 2 minutes btw ; )")]
        public float holdDuration = 3f;

        [Header("you gotta see what yah gotta see xd")]
        public GameObject progressBar;
        [Tooltip("have it image type to filled just like how i filled your MOM ")]
        public Image Fill;

        [Header("HOLY SHIT IT WORK what now tho ?")]
        public UnityEvent onSuccessI;

        [Tooltip("yo keep me or nah lol")]
        public bool destroyTriggerAfter = true;

        private bool playerInRange = false;
        private bool alreadyTrigger = false;
        private float cHtime = 0f;

        private IInputProvider playerInput;
        private PlayerInventory playerInventory;
        private List<ItemData> originalRequirements = new List<ItemData>();

        private void Awake()
        {
            originalRequirements = new List<ItemData>(requiredTool);
        }

        private void Start()
        {
            StateMem state = GetComponent<StateMem>();
            if (state != null)
            {
                int savecount = state.LP(requiredTool.Count);
                while (requiredTool.Count > savecount)
                {
                    requiredTool.RemoveAt(0);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !alreadyTrigger)
            {
                playerInRange = true;
                playerInput = other.GetComponent<IInputProvider>();
                playerInventory = other.GetComponent<PlayerInventory>();
                Debug.Log($"Hold 'E' to {actionText} (requires {requiredTool.Count} items)");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                playerInput = null;
                playerInventory = null;
                alreadyTrigger = false;
                ResetHold();
            }
        }

        private void Update()
        {
            if (!playerInRange || alreadyTrigger || playerInput == null || playerInventory == null) return;

            if (requiredTool.Count <= 0)
            {
                CompleteInteraction();
                return;
            }

            int activeSlot = playerInventory.activeSlotIndex;
            if (activeSlot < 0 || activeSlot >= playerInventory.hotbarSlot.Length) return;

            ItemData currentHeldItem = playerInventory.hotbarSlot[activeSlot];

            // If we are holding the right item and pressing E
            if (currentHeldItem != null && requiredTool.Contains(currentHeldItem) && playerInput.GetInteractHeld())
            {
                cHtime += Time.deltaTime;
                if (progressBar != null) progressBar.SetActive(true);
                if (Fill != null) Fill.fillAmount = cHtime / holdDuration;

                if (cHtime >= holdDuration)
                {
                    // Item Delivered!
                    requiredTool.Remove(currentHeldItem);
                    playerInventory.RemoveItem(currentHeldItem);
                    ResetHold();

                    if (requiredTool.Count > 0)
                    {
                        Debug.Log($"Accepted {currentHeldItem.iN}. Still need {requiredTool.Count} more.");
                        ShowMonologue("partial_success");
                        if (TryGetComponent<StateMem>(out StateMem mem)) mem.SP(requiredTool.Count);
                    }
                    else
                    {
                        CompleteInteraction();
                    }
                }
            }
            else if (cHtime > 0)
            {
                ResetHold(); // Player let go early or switched items
            }
        }

        private void CompleteInteraction()
        {
            alreadyTrigger = true;
            ResetHold();
            Debug.Log("Success! All required items delivered!");

            if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
            {
                GameManager.Instance.timeManager.UpdateTime(timeCostInMinutes);
            }

            onSuccessI?.Invoke();

            if (!destroyTriggerAfter)
            {
                requiredTool = new List<ItemData>(originalRequirements);
                return;
            }

            StateMem state = GetComponent<StateMem>();
            if (state != null)
            {
                state.destroyNremember();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void ResetHold()
        {
            cHtime = 0f;
            if (Fill != null) Fill.fillAmount = 0f;
            if (progressBar != null) progressBar.SetActive(false);
        }

        private void ShowMonologue(string textKey)
        {
            if (GameManager.Instance != null && GameManager.Instance.monologueUI != null)
            {
                GameManager.Instance.monologueUI.ST(textKey);
            }
        }
    }
}