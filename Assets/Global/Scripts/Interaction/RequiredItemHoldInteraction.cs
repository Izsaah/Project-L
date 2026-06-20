using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.Inventory;
using ProjectL.Scripts.Interface;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//todo: check why you can't trigger again even thought the the trigger is still alive 
namespace ProjectL.Global.Script.interaction
{
    public class RequiredItemHoldInteraction : MonoBehaviour
    {
        [Header("yo what are you holding : )")]
        public ItemData requiredTool;
        [Header("my record is 2 minutes btw ; )")]
        public float holdDuration = 3f;
        public int timeCostInMinutes = 30;

        [Header("you gotta see what yah gotta see xd")]
        public GameObject progressBar;
        [Tooltip("have it image type to filled just like how i filled your MOM ")]
        public Image Fill;

        [Header("climaz momemnt lol")]
        public UnityEvent onSuccuessI;
        public bool destroyTriggerAfter = true;

        private bool playerInRange = false;
        private bool alreadyTrigger = false;
        private float cHtime = 0f;

        private IInputProvider playerInput;
        private PlayerInventory playerInventory;

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
                ResetHold();
            }
        }
        private void Update()
        {
            if (playerInRange && !alreadyTrigger && playerInput != null)
            {
                ItemData cHItem = playerInventory.hotbarSlot[playerInventory.activeSlotIndex];
                if (cHItem == requiredTool && playerInput.GetInteractHeld())
                {
                    cHtime += Time.deltaTime;
                    if (progressBar != null) progressBar.SetActive(true);
                    if (Fill != null) Fill.fillAmount = cHtime / holdDuration;
                    if (cHtime >= holdDuration)
                    {
                        CompletionAction();
                    }
                }
                else if (cHtime > 0)
                {
                    ResetHold();

                }
            }
        }

        private void CompletionAction()
        {
            alreadyTrigger = true;
            ResetHold();
            if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
            {
                GameManager.Instance.timeManager.UpdateTime(timeCostInMinutes);
            }
            onSuccuessI?.Invoke();
            if (destroyTriggerAfter)
            {
                StateMem state = GetComponent<StateMem>(); if (state != null)
                {
                    state.destroyNremember();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        private void ResetHold()
        {
            cHtime = 0f;
            if (Fill != null) Fill.fillAmount = 0f;
            if (progressBar != null) progressBar.SetActive(false);
        }
    }
}