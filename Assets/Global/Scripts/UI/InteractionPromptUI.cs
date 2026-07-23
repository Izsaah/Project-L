using UnityEngine;

namespace ProjectL.Global.Scripts.UI
{
    public class InteractionPromptUI : MonoBehaviour
    {
        public static InteractionPromptUI Instance;

        [Tooltip("Drag your 'Press E' UI GameObject here")]
        public GameObject pressEUI;

        [Tooltip("Drag your 'Hold E' UI GameObject here")]
        public GameObject holdEUI;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            HideAll(); // Hide automatically when game starts
        }

        public void ShowPressE()
        {
            HideAll(); // Ensure we don't show both at the same time!
            if (pressEUI != null) pressEUI.SetActive(true);
        }

        public void ShowHoldE()
        {
            HideAll();
            if (holdEUI != null) holdEUI.SetActive(true);
        }

        public void HideAll()
        {
            if (pressEUI != null) pressEUI.SetActive(false);
            if (holdEUI != null) holdEUI.SetActive(false);
        }
    }
}
