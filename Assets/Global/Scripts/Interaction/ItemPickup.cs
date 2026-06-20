using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.Inventory;
using ProjectL.Scripts.Interface;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
namespace ProjectL.Global.Script.interaction
{
    public class ItemPickup : MonoBehaviour
    {   //todo make a meta data place for these thing
        [Header("Runtime Persistence")]
        [Tooltip("DO NOT TOUCH YOU STUPID FACK")]
        public string runtimeDropID = "";
        [Header("what tf is this bruh ?")]
        public ItemData bluePrint;

        private bool playerInRange = false;
        private PlayerInventory localI;

        private IInputProvider playerInput;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = true;

                localI = other.GetComponent<PlayerInventory>();
                playerInput = other.GetComponent<IInputProvider>();

                Debug.Log("TOUCH ME MF WOOOOOO" + bluePrint.iN);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerInRange = false;
                localI = null;
                playerInput = null;
            }
        }

        private void Update()
        {
            if (playerInRange && playerInput != null && playerInput.GetInteract())
            {
                bool wasAdded = localI.AddItem(bluePrint);
                if (wasAdded)
                {
                    if (bluePrint.size == IS.h)
                    {
                        if (GameManager.Instance != null && GameManager.Instance.monologueUI != null)
                        {
                            GameManager.Instance.monologueUI.ST("heavy_item");
                        }
                    }
                    StateMem state = GetComponent<StateMem>();
                    if (state != null)
                    {
                        state.destroyNremember();
                    }
                    else if (!string.IsNullOrEmpty(runtimeDropID))
                    {
                        if (GameManager.Instance != null && GameManager.Instance.droppedItems != null)
                        {
                            GameManager.Instance.droppedItems.RemoveDroppedItem(runtimeDropID);
                        }
                        Destroy(gameObject);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }

                }
                else
                {
                    Debug.Log("i'm gonna vomit xd please no" + bluePrint);
                }
            }
        }

    }

}
