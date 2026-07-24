using UnityEngine;

namespace ProjectL.Global.Scripts.Movement
{
    public class NPCDestroyer : MonoBehaviour
    {
        [Tooltip("Destroys all active NPCs in the scene that have the RaycastNPCWalker script.")]
        public void DestroyAllWalkingNPCs()
        {
            // Find every NPC currently in the scene
            RaycastNPCWalker[] allWalkingNPCs = FindObjectsOfType<RaycastNPCWalker>();

            // Loop through them and destroy their GameObjects
            foreach (RaycastNPCWalker npc in allWalkingNPCs)
            {
                if (npc != null)
                {
                    Destroy(npc.gameObject);
                }
            }
        }
    }
}
