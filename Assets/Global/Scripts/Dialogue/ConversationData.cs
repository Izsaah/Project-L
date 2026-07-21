using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
namespace ProjectL.Global.Script.Dialogue
{
    [CreateAssetMenu(fileName = "Conversation", menuName = "ProjectL/ConversationDB")]
    public class ConversationData : ScriptableObject
    {
        [System.Serializable]
        public struct DialogueLine
        {
            [Tooltip("WHO ARE YOU BROTHER WHOOOOOOOOO")]
            public string NPCName;
            [TextArea(1, 5)]
            public string dialogueText;
            
            [Header("Audio Settings")]
            public AudioClip audioClip;
        }
        [Header("THE SEQUENCEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE")]
        public List<DialogueLine> lines = new List<DialogueLine>();
    }

}
