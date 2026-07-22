using UnityEngine;
using TMPro;

namespace ProjectL.Global.Scripts.Objectives
{
    public class ObjectiveManager : MonoBehaviour
    {
        [Tooltip("Drag your UI Text (TextMeshPro) here.")]
        public TextMeshProUGUI objectiveText;

        public static ObjectiveManager Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // Set default objective on start
            UpdateObjective("Hãy đến nhà văn hóa để nhận nhiệm vụ.");
        }

        // Call this from UnityEvents (like your conversation triggers)
        public void UpdateObjective(string newObjective)
        {
            if (objectiveText != null)
            {
                objectiveText.text = newObjective;
            }
            else
            {
                Debug.LogWarning("Objective Manager: Text is not assigned in the inspector!", this);
            }
        }
    }
}
