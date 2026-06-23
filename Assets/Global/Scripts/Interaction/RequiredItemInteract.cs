using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.Inventory;
using ProjectL.Scripts.Interface;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
//todo: check why the trigger still alive but can't retrigger with the same items ?
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
        private IInputProvider playerInput;
        private PlayerInventory playerInventory;
        private bool alreadyTrigger = false;


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
                Debug.Log($"Press 'E' to {actionText} (requires {requiredTool.Count} items)");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                playerInput = null;
                playerInventory = null;
            }
        }

        private void Update()
        {
            if (!playerInRange || alreadyTrigger || playerInput == null || playerInventory == null)
            {
                return;
            }

            if (!playerInput.GetInteract())
            {
                return;
            }

            if (requiredTool.Count <= 0)
            {
                CompleteInteraction();
                return;
            }

            int activeSlot = playerInventory.activeSlotIndex;

            if (activeSlot < 0 || activeSlot >= playerInventory.hotbarSlot.Length)
            {
                ShowMonologue("missing_tool");
                return;
            }

            ItemData currentHeldItem = playerInventory.hotbarSlot[activeSlot];

            if (currentHeldItem == null || !requiredTool.Contains(currentHeldItem))
            {
                ShowMonologue("missing_tool");
                return;
            }

            requiredTool.Remove(currentHeldItem);
            playerInventory.hotbarSlot[activeSlot] = null;

            if (requiredTool.Count > 0)
            {
                Debug.Log($"Accepted {currentHeldItem.iN}. Still need {requiredTool.Count} more.");
                ShowMonologue("partial_success");
                if (TryGetComponent<StateMem>(out StateMem mem)) mem.SP(requiredTool.Count);
                return;
            }

            CompleteInteraction();
        }

        private void CompleteInteraction()
        {
            alreadyTrigger = true;

            Debug.Log("Success! All required items delivered!");

            if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
            {
                GameManager.Instance.timeManager.UpdateTime(timeCostInMinutes);
            }

            onSuccessI?.Invoke();

            if (!destroyTriggerAfter)
            {
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
