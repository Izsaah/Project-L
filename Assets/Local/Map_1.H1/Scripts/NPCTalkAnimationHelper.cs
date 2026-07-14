using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Script.Dialogue{
public class NPCTalkAnimationHelper : MonoBehaviour
{
    public Animator myAnimator;
    
    [Tooltip("Must exactly match the NPCName in your ConversationData asset (e.g., 'lolllllllllllllllllllllllllllllllll')")]
    public string myNPCName; 
    void Update()
    {
        // Check if the dialogue UI is open
        if (DialogueUIManager.Instance != null && DialogueUIManager.Instance.dialogueBox.activeInHierarchy)
        {
            // Check if THIS specific NPC is the one currently speaking
            if (DialogueUIManager.Instance.speakerNameText.text == myNPCName)
            {
                myAnimator.SetBool("IsTalking", true);
            }
            else
            {
                myAnimator.SetBool("IsTalking", false);
            }
        }
        else
        {
            // Dialogue is closed
            if (myAnimator != null)
            {
                myAnimator.SetBool("IsTalking", false);
            }
        }
    }
}
}