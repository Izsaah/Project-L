using UnityEngine;

namespace ProjectL.Global.Scripts.Objectives
{
    [RequireComponent(typeof(BoxCollider))]
    public class QuestTrigger : MonoBehaviour
    {
        [Tooltip("The text to show when the player enters this trigger.")]
        public string nextObjectiveText = "Go to somewhere else";
        
        [Tooltip("If true, the trigger turns off after one use.")]
        public bool triggerOnce = true;
        
        private bool hasTriggered = false;

        void OnTriggerEnter(Collider other)
        {
            if (hasTriggered) return;

            // Check if the player walked into this
            if (other.CompareTag("Player"))
            {
                if (ObjectiveManager.Instance != null)
                {
                    ObjectiveManager.Instance.UpdateObjective(nextObjectiveText);
                }
                
                if (triggerOnce)
                {
                    hasTriggered = true;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
