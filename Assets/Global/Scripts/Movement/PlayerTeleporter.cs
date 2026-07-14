using UnityEngine;

namespace ProjectL.Global.Script.Movement 
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [Header("Teleport Settings")]
        [Tooltip("Drag the Empty GameObject showing where the player should end up!")]
        public Transform destination;

        // Call this from your interaction event!
        public void TeleportNow()
        {
            if (destination == null)
            {
                Debug.LogWarning("Teleporter: No destination assigned!");
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();
                
                if (cc != null)
                {
                    // You MUST disable the CharacterController before teleporting in Unity!
                    cc.enabled = false;
                    
                    player.transform.position = destination.position;
                    player.transform.rotation = destination.rotation; // This also turns the player to face the right way!
                    
                    cc.enabled = true;
                }
                else
                {
                    // Fallback just in case
                    player.transform.position = destination.position;
                    player.transform.rotation = destination.rotation;
                }
            }
        }
    }
}
