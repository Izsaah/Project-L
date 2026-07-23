using UnityEngine;

namespace ProjectL.Global.Script.CowRescue
{
    public class FloodController : MonoBehaviour
    {
        [Header("Flood Settings")]
        [Tooltip("The speed at which the water rises on the Y axis.")]
        public float riseSpeed = 0.5f;
        [Tooltip("The maximum Y position the water will reach.")]
        public float maxYPosition = 5f;
        [Tooltip("If true, the water starts rising immediately on Awake/Enable.")]
        public bool autoStart = false;

        private bool isRising = false;

        private void OnEnable()
        {
            if (autoStart)
            {
                isRising = true;
            }
        }

        public void StartFlood()
        {
            isRising = true;
            gameObject.SetActive(true); // Ensure it's active
        }

        public void StopFlood()
        {
            isRising = false;
        }

        private void Update()
        {
            if (isRising)
            {
                Vector3 currentPos = transform.position;
                if (currentPos.y < maxYPosition)
                {
                    currentPos.y += riseSpeed * Time.deltaTime;
                    
                    // Clamp to max Y
                    if (currentPos.y > maxYPosition)
                    {
                        currentPos.y = maxYPosition;
                        isRising = false; // Stop rising once max height reached
                    }
                    
                    transform.position = currentPos;
                }
            }
        }
    }
}
