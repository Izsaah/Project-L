using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Audio;
using ProjectL.Global.Script.Inventory;
using ProjectL.Global.Script.Player;
using ProjectL.Scripts.Interface;
using Unity.Burst.Intrinsics;
using Unity.Mathematics;
using UnityEngine;
namespace ProjectL.Global.Script.Movement
{
    //todo: fix the shift speed when jump and drop shift lost momentum and it lead to a really weird velocity movement.
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class PlayerMovement : MonoBehaviour
    {

        [Header("References")]
        [SerializeField] private MonoBehaviour inputSource;

        [Header("Settings")]
        [SerializeField] private MovementSettings settings;

        [Header("Audio Integration")]
        [SerializeField] private SurfaceDetector surfaceDetector;
        [Tooltip("How far the player needs to walk before playing a step sound.")]
        [SerializeField] private float stepDistance = 2.0f;

        private IInputProvider inputProvider;
        private CharacterController controller;
        private PlayerInventory playerInventory;

        private Vector3 velocity;
        private float cStamina;
        public bool isExhausted = false;
        private float accumulatedDistance = 0f;

        private void Awake()
        {
            inputProvider = inputSource as IInputProvider;
            if (inputProvider == null)
            {
                Debug.LogError("input source must implement IInputProvider");
            }
            controller = GetComponent<CharacterController>();
            playerInventory = GetComponent<PlayerInventory>();
            cStamina = settings.maxStamina;
        }

        // Update is called once per frame
        private void Update()
        {
            // 1. Process Physical Forces & Input Direction via Utilities
            velocity.y = MUtils.gCheck(controller, velocity.y);
            bool carry = playerInventory.IsHoldingHeavyItem();

            Vector3 hVelocity = MUtils.CHM(transform, settings, inputProvider, ref cStamina, ref isExhausted, Time.deltaTime, carry);
            velocity.y = MUtils.CJAG(velocity.y, settings, inputProvider, controller);

            // 2. Execute Translation
            Vector3 fVelocity = hVelocity + velocity;
            controller.Move(fVelocity * Time.deltaTime);

            // 3. Evaluate Step Sound Boundaries Natively
            if (MUtils.CalculateStepProgress(hVelocity, controller.isGrounded, ref accumulatedDistance, stepDistance, Time.deltaTime))
            {
                if (surfaceDetector != null)
                {
                    surfaceDetector.DSAPA();
                }
            }
        }
    }

    [System.Serializable]
    public class MovementSettings
    {

        public float moveSpeed = 1.5f;
        public float gravity = -9.81f;
        public float jumpHeight = 1.5f;
        public float sprintSpeed = 5f;

        [Header("Stamina")]
        public float maxStamina = 100f;
        public float drainRate = 20f;
        public float restoreRate = 15f;
        public float ERT = 80f;
    }



}