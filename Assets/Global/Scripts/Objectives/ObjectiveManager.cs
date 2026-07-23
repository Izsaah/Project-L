using UnityEngine;
using TMPro;

namespace ProjectL.Global.Scripts.Objectives
{
    public class ObjectiveManager : MonoBehaviour
    {
        [Tooltip("Drag your UI Text (TextMeshPro) here.")]
        public TextMeshProUGUI objectiveText;

        public static ObjectiveManager Instance;
        
        private bool isHiddenByCutscene = false;

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
                // Only turn the text on if a cutscene isn't actively playing
                if (!isHiddenByCutscene)
                {
                    objectiveText.gameObject.SetActive(true);
                }
                objectiveText.text = newObjective;
            }
            else
            {
                Debug.LogWarning("Objective Manager: Text is not assigned in the inspector!", this);
            }
        }

        public void HideObjective()
        {
            isHiddenByCutscene = true;
            if (objectiveText != null) objectiveText.gameObject.SetActive(false);
        }

        public void ShowObjective()
        {
            isHiddenByCutscene = false;
            if (objectiveText != null) objectiveText.gameObject.SetActive(true);
        }
    }
}
