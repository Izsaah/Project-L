
using System.Collections;
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
        //let hope this also fix one of the problem lol
        private IEnumerator Start()
        {
            GameObject player = null;

            // Wait until the player is actually in the scene
            while (player == null)
            {
                player = GameObject.FindWithTag("Player");
                yield return null;
            }

            // Now it is 100% safe to do this
            playerInventory = player.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.OnInventoryChanged += updateUI;
                updateUI();
            }
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
