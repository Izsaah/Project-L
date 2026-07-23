using UnityEngine;
using ProjectL.Global.Script.Dialogue;

public class DisableAfterDialogue : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;
    public BoxCollider boxCollider;

    public void DisableComponents()
    {
        if (dialogueTrigger != null) dialogueTrigger.enabled = false;
        if (boxCollider != null) boxCollider.enabled = false;
    }
}
