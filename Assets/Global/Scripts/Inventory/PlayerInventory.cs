using System;
using ProjectL.Global.Script.interaction;
using ProjectL.Global.Script.Player;
using ProjectL.Scripts.Interface;
using UnityEngine;


//todo: fix item ui when drop and pickup item again and then spam the same slot as the slot, it doesnt show the block ui
namespace ProjectL.Global.Script.Inventory
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Hotbar")]
        public ItemData[] hotbarSlot = new ItemData[3];
        [Header("FPS visual")]
        public Transform handpoint;
        private GameObject currentHeldModel;

        public int activeSlotIndex = 0;
        private IInputProvider inputHandler;
        private int lastEIndex = -1;
        public event Action OnInventoryChanged;
        private void Awake()
        {
            inputHandler = GetComponent<IInputProvider>();
        }

        private void Update()
        {
            HandleSlotSelection();
            if (inputHandler.GetDropInput())
            {
                DropCurrentItem();
            }
        }

        private void HandleSlotSelection()
        {
            int pressedSlot = inputHandler.GetHotBar();

            if (pressedSlot != -1)
            {
                activeSlotIndex = pressedSlot;
                EquipItem(activeSlotIndex);
            }
        }

        private void EquipItem(int index)
        {
            if (index == lastEIndex) return;

            lastEIndex = index;

            ItemData iTE = hotbarSlot[index];
            if (currentHeldModel != null)
            {
                Destroy(currentHeldModel);
            }

            if (iTE != null)
            {
                Debug.Log("Equipped: " + iTE.iN);
                if (iTE.fab != null)
                {
                    currentHeldModel = Instantiate(iTE.fab, handpoint.position, handpoint.rotation, handpoint);


                    currentHeldModel.transform.localEulerAngles = iTE.rotOS;

                    Rigidbody handRb = currentHeldModel.GetComponent<Rigidbody>();
                    if (handRb != null)
                    {
                        Destroy(handRb);
                    }


                    Collider[] handColliders = currentHeldModel.GetComponents<Collider>();
                    foreach (Collider col in handColliders)
                    {
                        Destroy(col);
                    }
                }
            }
            else
            {
                Debug.Log("Equipped an empty slot!");
            }
        }

        public bool IsHoldingHeavyItem()
        {
            ItemData currentItem = hotbarSlot[activeSlotIndex];

            return currentItem != null && currentItem.size == IS.h;
        }

        public bool AddItem(ItemData itemToAdd)
        {
            int requiredSlots = (int)itemToAdd.size;

            int cES = 0;
            int startIndex = -1;

            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                if (hotbarSlot[i] == null)
                {
                    if (cES == 0) startIndex = i;
                    cES++;
                    if (cES == requiredSlots)
                    {

                        for (int j = startIndex; j < startIndex + requiredSlots; j++)
                        {
                            hotbarSlot[j] = itemToAdd;
                        }


                        Debug.Log($"Added {itemToAdd.iN} (Size: {requiredSlots})");
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                }
                else
                {
                    cES = 0;
                    startIndex = -1;
                }
            }
            Debug.Log($"Not enough space to hold {itemToAdd.iN}!");
            return false;
        }

        private void DropCurrentItem()
        {
            ItemData data = hotbarSlot[activeSlotIndex];
            if (data == null) return;
            Vector3 safeSpawnDirection = new Vector3(handpoint.forward.x, 0f, handpoint.forward.z).normalized;
            Vector3 dropPos = handpoint.position + safeSpawnDirection * 1.2f;

            GameObject droppedObject = Instantiate(data.fab, dropPos, handpoint.rotation);
            Rigidbody rb = droppedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(handpoint.forward * 3f, ForceMode.Impulse);
            }
            if (Core.GameMaster.GameManager.Instance != null && Core.GameMaster.GameManager.Instance.droppedItems != null)
            {
                string ID = Core.GameMaster.GameManager.Instance.droppedItems.RND(data, dropPos, droppedObject);
                ItemPickup pickup = droppedObject.GetComponent<ItemPickup>();
                if (pickup != null)
                {
                    pickup.runtimeDropID = ID;
                }
            }
            if (currentHeldModel != null)
            {
                Destroy(currentHeldModel);
            }
            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                if (hotbarSlot[i] == data)
                {
                    hotbarSlot[i] = null;
                }
            }
            Debug.Log("Dropped: " + data.iN);
            OnInventoryChanged?.Invoke();
        }
        //the ai told me this will work so i believe it will work : )
        public void RemoveItem(ItemData itemToRemove)
        {
            if (itemToRemove == null) return;

            // Figure out exactly how many slots this one item takes up (Normal = 1, Heavy = 2, etc.)
            int requiredSlots = (int)itemToRemove.size;

            // Safety check just in case you ever have an item size of 0
            if (requiredSlots < 1) requiredSlots = 1;

            int slotsCleared = 0;

            // Loop through the hotbar
            for (int i = 0; i < hotbarSlot.Length; i++)
            {
                if (hotbarSlot[i] == itemToRemove)
                {
                    hotbarSlot[i] = null; // Delete the item from this slot
                    slotsCleared++;

                    // THE CRITICAL FIX: Stop the loop once we've deleted exactly ONE item!
                    if (slotsCleared >= requiredSlots)
                    {
                        break;
                    }
                }
            }

            // If we just removed the item we were currently holding, destroy the visual model
            if (hotbarSlot[activeSlotIndex] == null && currentHeldModel != null)
            {
                Destroy(currentHeldModel);
                lastEIndex = -1; // Reset so EquipItem knows to update next time we switch slots
            }

            // Tell the UI to update!
            OnInventoryChanged?.Invoke();
        }
    }
}