using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using ProjectL.Global.Script.Dialogue;

namespace ProjectL.Local.Map_1_H1
{
    public class ForceEndDialogueOnExit : MonoBehaviour
    {
        private void OnTriggerExit(Collider other)
        {
            // When the player walks far away and exits the collider
            if (other.CompareTag("Player"))
            {
                // Check if the UI Manager exists and a conversation is currently showing on screen
                if (DialogueUIManager.Instance != null && DialogueUIManager.Instance.dialogueBox.activeInHierarchy)
                {
                    // Use Reflection to grab your private "EndConversation" method from the UI Manager
                    MethodInfo endMethod = typeof(DialogueUIManager).GetMethod("EndConversation", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (endMethod != null)
                    {
                        // Force the private method to run. 
                        // This will cleanly close the UI, set pCC to null, and fire your OnConversationDone callback!
                        endMethod.Invoke(DialogueUIManager.Instance, null);
                    }
                }
            }
        }
    }
}