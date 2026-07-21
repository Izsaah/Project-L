using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.Inventory;
using ProjectL.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectL.Global.Script.interaction
{
    public class RequiredItemInteract : MonoBehaviour
    {
        [Header("HOW LONG HAVE IT BEEN HAHAHAHAHAH")]
        public int timeCostInMinutes = 15;

        [Header("YOOO WHAT IN YO TOOL BOX DUMBASS!!!!")]
        public List<ItemData> requiredTool = new List<ItemData>();
        public String actionText = "";

        [Header("HOLY SHIT IT WORK what now tho ?")]
        public UnityEvent onSuccessI;

        [Tooltip("yo keep me or nah lol")]
        public bool destroyTriggerAfter = true;

        private bool playerInRange = false;
        private bool alreadyTrigger = false;

        private IInputProvider playerInput;
        private PlayerInventory playerInventory;
        private List<ItemData> originalRequirements = new List<ItemData>();
        [Header("Penalty for being a dummy")]
        public int penaltyTimeInMinutes = 5;

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
            }
        }

        private void Update()
        {
            if (!playerInRange || alreadyTrigger || playerInput == null || playerInventory == null) return;

            if (!playerInput.GetInteract()) return;

            if (requiredTool.Count <= 0)
            {
                CompleteInteraction();
                return;
            }

            int activeSlot = playerInventory.activeSlotIndex;
            if (activeSlot < 0 || activeSlot >= playerInventory.hotbarSlot.Length)
            {
                GameManager.Instance.timeManager.UpdateTime(penaltyTimeInMinutes);
                return;
            }

            ItemData currentHeldItem = playerInventory.hotbarSlot[activeSlot];

            if (currentHeldItem == null || !requiredTool.Contains(currentHeldItem))
            {
                GameManager.Instance.timeManager.UpdateTime(penaltyTimeInMinutes);
                return;
            }

            // Item Delivered!
            requiredTool.Remove(currentHeldItem);
            playerInventory.RemoveItem(currentHeldItem);

            if (requiredTool.Count > 0)
            {

                if (TryGetComponent<StateMem>(out StateMem mem)) mem.SP(requiredTool.Count);
            }
            else
            {
                CompleteInteraction();
            }
        }

        private void CompleteInteraction()
        {
            alreadyTrigger = true;


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

        private void ShowMonologue(string textKey)
        {
            if (GameManager.Instance != null && GameManager.Instance.monologueUI != null)
            {
                GameManager.Instance.monologueUI.ST(textKey);
            }
        }
    }
}