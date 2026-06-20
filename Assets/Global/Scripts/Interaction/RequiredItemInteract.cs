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
        public ItemData requiredTool;
        public String actionText = "";

        [Header("HOLY SHIT IT WORK what now tho ?")]
        public UnityEvent onSuccessI;

        [Tooltip("yo keep me or nah lol")]
        public bool destroyTriggerAfter = true;

        private bool playerInRange = false;
        private IInputProvider playerInput;
        private PlayerInventory playerInventory;
        private bool alreadyTrigger = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !alreadyTrigger)
            {
                playerInRange = true;
                playerInput = other.GetComponent<IInputProvider>();
                playerInventory = other.GetComponent<PlayerInventory>();
                Debug.Log($"Press 'E' to {actionText}(requires{requiredTool.iN})");
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
            if (playerInRange && !alreadyTrigger && playerInput != null && playerInput.GetInteract())
            {
                ItemData cHItem = playerInventory.hotbarSlot[playerInventory.activeSlotIndex];
                if (cHItem == requiredTool)
                {
                    alreadyTrigger = true;
                    Debug.Log("Success! lol");
                    if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
                    {
                        GameManager.Instance.timeManager.UpdateTime(timeCostInMinutes);
                    }

                    onSuccessI?.Invoke();

                    if (destroyTriggerAfter)
                    {
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
                }
                else
                {
                    GameManager.Instance.monologueUI.ST("missing_tool");
                }
            }
        }
    }
}
