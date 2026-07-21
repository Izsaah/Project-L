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
        
        [Header("Audio Setup")]
        public AudioSource dialogueAudioSource;

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
            var line = pCC.lines[cLI];
            speakerNameText.text = line.NPCName;
            dBT.text = line.dialogueText;

            if (dialogueAudioSource != null)
            {
                if (dialogueAudioSource.isPlaying)
                {
                    dialogueAudioSource.Stop();
                }

                if (line.audioClip != null)
                {
                    dialogueAudioSource.clip = line.audioClip;
                    dialogueAudioSource.Play();
                }
            }
        }
        public void EndConversation()
        {
            dialogueBox.SetActive(false);
            pCC = null;

            onConversationEnd?.Invoke();
        }
    }
}
