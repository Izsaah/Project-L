using UnityEngine;

namespace ProjectL.Global.Script.CowRescue
{
    public class SafeZoneTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (CowRescueManager.Instance != null)
                {
                    CowRescueManager.Instance.PlayerArrivedAtSafeZone();
                }
            }
        }
    }
}
