using UnityEngine;

namespace ProjectL.Global.Scripts.Objectives
{
    public class LocalObjectiveTrigger : MonoBehaviour
    {
        [Tooltip("If true, this object will automatically become the waypoint when the scene loads.")]
        public bool setAsWaypointOnStart = false;

        void Start()
        {
            if (setAsWaypointOnStart)
            {
                SetNewWaypoint(this.transform);
            }
        }

        // Call this from your Unity Event on the NPC!
        public void SetNewObjective(string nextObjective)
        {
            if (ObjectiveManager.Instance != null)
            {
                ObjectiveManager.Instance.UpdateObjective(nextObjective);
            }
            else
            {
                Debug.LogWarning("ObjectiveManager is not loaded in the scene!");
            }
        }

        // Call this from your Unity Event to set the 3D target location!
        public void SetNewWaypoint(Transform newTarget)
        {
            if (WaypointMarker.Instance != null)
            {
                WaypointMarker.Instance.SetTarget(newTarget);
            }
            else
            {
                Debug.LogWarning("WaypointMarker is not loaded in the scene!");
            }
        }
    }
}
