using UnityEngine;

namespace ProjectL.Global.Scripts.Movement
{
    public class RaycastNPCWalker : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float speed = 3f;
        [Tooltip("Check this if your animation moves the character forward on its own!")]
        public bool letAnimationMoveCharacter = false;
        
        [Header("Animation Settings")]
        [Tooltip("Optional: Drag the Animator component here")]
        public Animator animator;
        [Tooltip("The name of the boolean parameter in your Animator that handles walking")]
        public string walkBoolParameter = "IsWalking";
        
        [Header("Treadmill Fix")]
        [Tooltip("If your animation walks out of the collider, drag the moving bone (like Armature or Root) here to lock it in place!")]
        public Transform lockBoneToTreadmill;
        private Vector3 startBoneLocalPos;


        [Header("Raycast Settings")]
        [Tooltip("How far ahead the NPC 'sees'")]
        public float lookDistance = 1.5f; 
        
        [Tooltip("An offset so the ray shoots from the chest/head, not the floor")]
        public Vector3 rayOffset = new Vector3(0, 1f, 0); 
        
        [Tooltip("Which layers should count as obstacles?")]
        public LayerMask obstacleLayer = Physics.DefaultRaycastLayers;

        void Start()
        {
            if (lockBoneToTreadmill != null)
            {
                startBoneLocalPos = lockBoneToTreadmill.localPosition;
            }
        }

        void Update()
        {


            // Calculate where the ray should start
            Vector3 rayStart = transform.position + rayOffset;
            
            // Draw a visible red line in the Unity Editor to help you test
            Debug.DrawRay(rayStart, transform.forward * lookDistance, Color.red);

            bool isMoving = false;

            // Shoot the Raycast forward
            // By adding QueryTriggerInteraction.Collide, the raycast will successfully hit colliders marked as "Is Trigger"
            if (Physics.Raycast(rayStart, transform.forward, lookDistance, obstacleLayer, QueryTriggerInteraction.Collide))
            {
                // The ray hit something! Turn a random amount to avoid corner traps
                float randomTurn = Random.Range(90f, 270f);
                transform.Rotate(0, randomTurn, 0);
                
                // When turning instantly, technically we aren't moving forward this frame
                isMoving = false; 
            }
            else
            {
                // The path is clear, keep walking
                if (!letAnimationMoveCharacter)
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                }
                isMoving = true;
            }

            // Handle Animation if an Animator is assigned
            if (animator != null && !string.IsNullOrEmpty(walkBoolParameter))
            {
                animator.SetBool(walkBoolParameter, isMoving);
            }
        }

        // This built-in Unity method fires when the player clicks on the collider of this GameObject
        private void OnMouseDown()
        {
            // The player clicked the NPC, so we instantly destroy it!
            Destroy(gameObject);
        }

        void LateUpdate()
        {
            // The Animator moves the bones in Update. LateUpdate runs immediately after.
            // We forcefully overwrite the position of the bone back to where it started.
            // (GLB files often have weird local axes where Y or Z could be forward, so we lock everything to be safe!)
            if (lockBoneToTreadmill != null)
            {
                lockBoneToTreadmill.localPosition = startBoneLocalPos;
            }
        }
    }
}
