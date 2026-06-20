
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
namespace ProjectL.Global.Script.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("References")]
        public PlayerInventory playerInventory;

        [Header("UI Elements")]
        public Image[] slotIcons;

        private void Start()
        {
            playerInventory.OnInventoryChanged += updateUI;
            updateUI();
        }

        private void updateUI()
        {
            for (int i = 0; i < slotIcons.Length; i++)
            {
                ItemData item = playerInventory.hotbarSlot[i];
                if (item != null)
                {
                    slotIcons[i].sprite = item.i;
                    slotIcons[i].color = Color.white;
                }
                else
                {
                    slotIcons[i].sprite = null;
                    slotIcons[i].color = new Color(1f, 1f, 1f, 0.2f);
                }
            }
        }
        private void OnDestroy()
        {
            if (playerInventory != null)
            {
                playerInventory.OnInventoryChanged -= updateUI;
            }
        }
    }
}
