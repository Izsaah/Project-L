using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace ProjectL.Global.Script.Dialogue
{
    public class DialogueUIManager : MonoBehaviour
    {
        public static DialogueUIManager Instance;

        [Header("UI references")]
        public GameObject dialogueBox;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI dBT;

        private ConversationData pCC;
        private int cLI = 0;
        private System.Action onConversationEnd;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            if (dialogueBox != null) dialogueBox.SetActive(false);
        }

        public void StartConversation(ConversationData convo, System.Action onComplete)
        {
            pCC = convo;
            cLI = 0;
            onConversationEnd = onComplete;

            dialogueBox.SetActive(true);
            DisplayLine();
        }
        public void AdvanceConversation()
        {
            cLI++;
            if (cLI < pCC.lines.Count)
            {
                DisplayLine();
            }
            else
            {
                EndConversation();
            }
        }
        private void DisplayLine()
        {
            speakerNameText.text = pCC.lines[cLI].NPCName;
            dBT.text = pCC.lines[cLI].dialogueText;
        }
        private void EndConversation()
        {
            dialogueBox.SetActive(false);
            pCC = null;

            onConversationEnd?.Invoke();
        }
    }
}
