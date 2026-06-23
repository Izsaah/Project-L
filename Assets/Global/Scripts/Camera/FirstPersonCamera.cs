using System.Collections;
using System.Collections.Generic;
using ProjectL.Scripts.Interface;
using UnityEngine;
namespace ProjectL.Global.Script.Camera
{
    public class FirstPersonCamera : MonoBehaviour
    {
        [Header("LOOK WHO BACK BACK AGAIN")]
        public Transform playerBody;
        private IInputProvider inputProvider;

        [Header("Settings")]
        public float mouseSens = 2f;

        private float xRotation = 0f;
        private void Awake()
        {
            inputProvider = GetComponentInParent<IInputProvider>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update()
        {
            if (inputProvider == null) return;
            Vector2 lookInput = inputProvider.GetLookDelta();

            float mX = lookInput.x * mouseSens;
            float mY = lookInput.y * mouseSens;

            xRotation -= mY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            Debug.Log($"Mouse Y: {mY} | Current X Angle: {xRotation}");
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mX);
        }
    }
}

