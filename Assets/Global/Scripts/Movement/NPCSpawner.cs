using UnityEngine;

namespace ProjectL.Global.Scripts.Movement
{
    public class NPCSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [Tooltip("The NPC prefab you want to spawn")]
        public GameObject npcPrefab;
        
        [Tooltip("The maximum number of these NPCs allowed in the scene at once.")]
        public int maxNPCsInScene = 1;
        
        private void Start()
        {
            // Count how many of these NPCs currently exist in the scene.
            // We do this by finding all objects that have the RaycastNPCWalker script.
            RaycastNPCWalker[] existingNPCs = FindObjectsOfType<RaycastNPCWalker>();
            
            if (existingNPCs.Length >= maxNPCsInScene)
            {
                // The max number of NPCs already exists in the scene. 
                // Do not spawn a new one, and destroy this spawner.
                Destroy(gameObject);
            }
            else
            {
                // We haven't reached the limit, so it's safe to spawn!
                if (npcPrefab != null)
                {
                    // Spawn the NPC exactly where this spawner is located, facing the same direction
                    Instantiate(npcPrefab, transform.position, transform.rotation);
                }
                
                // Destroy this spawner object after it has done its job
                Destroy(gameObject);
            }
        }
    }
}
