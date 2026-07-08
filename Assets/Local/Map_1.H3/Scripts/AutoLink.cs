using UnityEngine;
using UnityEngine.UI;
using ProjectL.Global.Script.Inventory;
using ProjectL.Global.Script.interaction;

// This forces Unity to make sure the main hold interaction script is also attached
namespace ProjectL.Local.Map_1_H3
{
    [RequireComponent(typeof(RequiredItemHoldInteraction))]
    public class AutoLink : MonoBehaviour
    {
        private void Start()
        {
            // Get the main interaction script on this object
            RequiredItemHoldInteraction interaction = GetComponent<RequiredItemHoldInteraction>();

            // Only do this if the slots are empty in the inspector
            if (interaction.progressBar == null)
            {
                // Scan for the Global Inventory UI
                InventoryUI globalUI = FindAnyObjectByType<InventoryUI>();

                if (globalUI != null)
                {
                    // Find the Progress Bar
                    Transform foundBar = globalUI.transform.Find("ProgressBar");
                    if (foundBar != null)
                    {
                        // Inject it into the main script!
                        interaction.progressBar = foundBar.gameObject;

                        // Find the Fill Image
                        Transform foundFill = foundBar.Find("Fill");
                        if (foundFill != null)
                        {
                            // Inject the Fill image into the main script!
                            interaction.Fill = foundFill.GetComponent<Image>();
                        }
                    }
                }
            }
        }
    }
}