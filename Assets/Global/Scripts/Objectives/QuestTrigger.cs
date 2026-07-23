using UnityEngine;

namespace ProjectL.Global.Scripts.Objectives
{
    [RequireComponent(typeof(BoxCollider))]
    public class QuestTrigger : MonoBehaviour
    {
        [Tooltip("The text to show when the player enters this trigger.")]
        public string nextObjectiveText = "Go to somewhere else";
        
        [Tooltip("The next object to point the waypoint to (leave empty to hide the waypoint!)")]
        public Transform nextWaypointTarget;
        
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

                if (WaypointMarker.Instance != null)
                {
                    // This will move the waypoint to the next target, OR hide it if you left the slot empty!
                    WaypointMarker.Instance.SetTarget(nextWaypointTarget);
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
