using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Scripts.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectL.Global.Script.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("WHAT THE FUCK ARE SYOUSA YING RBUH")]
        public ConversationData conversation;

        [Header("I NEED TO KNOWWWWWWWWWWWW THE ENDDDDDDDDDDDDDD")]
        public UnityEvent onChatFinished;

        private bool pIR = false;
        private bool iC = false;
        private IInputProvider pI;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                pIR = true;
                pI = other.GetComponent<IInputProvider>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                pIR = false;
                pI = null;
            }
        }

        private void Update()
        {
            if (pIR && pI != null && pI.GetInteract())
            {
                if (!iC)
                {
                    iC = true;

                    if (DialogueUIManager.Instance != null && conversation != null)
                    {
                        DialogueUIManager.Instance.StartConversation(conversation, OnConversationDone);
                    }
                }
                else
                {
                    if (DialogueUIManager.Instance != null)
                    {
                        DialogueUIManager.Instance.AdvanceConversation();
                    }
                }
            }
        }

        private void OnConversationDone()
        {
            iC = false;
            onChatFinished?.Invoke();
        }
    }
}