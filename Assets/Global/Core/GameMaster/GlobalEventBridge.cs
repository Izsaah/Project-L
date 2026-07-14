using UnityEngine;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.CutScenes;
using ProjectL.Global.Script.Audio;
using ProjectL.Global.Script.Inventory;

public class GlobalEventBridge : MonoBehaviour
{
    // The slot for Cutscenes!
    public void PlayCutscene(CutsceneDatabase cutsceneFile)
    {
        if (CutSceneUIManager.Instance != null)
        {
            CutSceneUIManager.Instance.ShowImage(cutsceneFile);
        }
    }

    // The slot for Audio!
    public void PlayGlobalSound(AudioData audioFile)
    {
        if (GameManager.Instance != null && GameManager.Instance.audioManager != null)
        {
            GameManager.Instance.audioManager.pSS(audioFile);
        }
    }
    public void GiveItemDirectly(ProjectL.Global.Script.Inventory.ItemData rewardItem)
    {
        // 1. Find the Player's inventory in the scene
        PlayerInventory pInv = FindObjectOfType<PlayerInventory>();

        if (pInv != null)
        {
            // 2. Magically shove the item into their pocket!
            bool wasAdded = pInv.AddItem(rewardItem);

            if (!wasAdded)
            {
                Debug.LogWarning("Inventory is full! Couldn't give the item.");
            }
        }
        else
        {
            Debug.LogWarning("GlobalBridge: Could not find the PlayerInventory in the scene!");
        }
    }
}
