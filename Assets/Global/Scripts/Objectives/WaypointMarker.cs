using UnityEngine;
using UnityEngine.UI;

namespace ProjectL.Global.Scripts.Objectives
{
    public class WaypointMarker : MonoBehaviour
    {
        public static WaypointMarker Instance;

        [Tooltip("The UI Image used as the waypoint icon")]
        public Image waypointIcon;
        
        [Tooltip("How high above the target to place the icon")]
        public Vector3 offset = new Vector3(0, 2f, 0);

        [Tooltip("The player's Transform. The waypoint hides when the player gets close.")]
        public Transform player;

        [Tooltip("Distance at which the waypoint automatically hides")]
        public float hideDistance = 3f;

        private Transform currentTarget;
        private bool isHiddenByMenu = false;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            if (waypointIcon != null) waypointIcon.enabled = false;
        }

        void Start()
        {
            // Try to find the player automatically if not assigned
            if (player == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) player = p.transform;
            }
        }

        void Update()
        {
            if (currentTarget == null || waypointIcon == null || Camera.main == null || isHiddenByMenu)
            {
                if (waypointIcon != null) waypointIcon.enabled = false;
                return;
            }

            // Hide the waypoint if the player is standing right next to the target
            if (player != null)
            {
                float distance = Vector3.Distance(player.position, currentTarget.position);
                if (distance <= hideDistance)
                {
                    // Clear the target so it turns off permanently until a new one is set!
                    currentTarget = null;
                    waypointIcon.enabled = false;
                    return;
                }
            }

            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTarget.position + offset);

            // screenPos.z > 0 means the object is in front of the camera
            if (screenPos.z > 0)
            {
                waypointIcon.enabled = true;
                waypointIcon.transform.position = screenPos;
            }
            else
            {
                waypointIcon.enabled = false;
            }
        }

        // Call this to change where the marker points
        public void SetTarget(Transform newTarget)
        {
            currentTarget = newTarget;
            
            if (newTarget != null && waypointIcon != null && !isHiddenByMenu)
            {
                waypointIcon.enabled = true;
            }
        }

        public void HideMarker()
        {
            isHiddenByMenu = true;
            if (waypointIcon != null) waypointIcon.enabled = false;
        }

        public void ShowMarker()
        {
            isHiddenByMenu = false;
        }
    }
}
