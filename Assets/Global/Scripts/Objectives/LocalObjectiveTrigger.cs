using UnityEngine;

namespace ProjectL.Global.Scripts.Objectives
{
    public class LocalObjectiveTrigger : MonoBehaviour
    {
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
    }
}
