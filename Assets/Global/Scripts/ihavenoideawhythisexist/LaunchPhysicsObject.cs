using UnityEngine;

namespace ProjectL.Global.Script.Interact
{
    public class LaunchPhysicsObject : MonoBehaviour
    {
        public Vector3 pushDirection = new Vector3(0, 5f, 10f);

        // We will call this function from the UnityEvent!
        public void Launch()
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(pushDirection, ForceMode.Impulse);
            }
        }
    }
}