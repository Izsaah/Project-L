using System.Collections;
using System.Collections.Generic;
using ProjectL.Scripts.Interface;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectL.Global.Script.Player
{
    public enum InputAction
    {//jump,left,right,front,back,running
        J, L, R, F, B, S, slot1, slot2, slot3, interact, drop, light, pause
    }

    [System.Serializable]
    public struct KeyBinding
    {
        public InputAction action;
        public KeyCode key;
    }

    public class PlayerInputHandler : MonoBehaviour, IInputProvider
    {
        [Header("Key Bind")]
        [SerializeField] private KeyBinding[] bind;

        private KeyCode MappedKey(InputAction action)
        {
            foreach (KeyBinding binding in bind)
            {
                if (binding.action == action)
                {
                    return binding.key;
                }
            }
            return KeyCode.None;
        }


        public bool GetJump()
        {
            return Input.GetKeyDown(MappedKey(InputAction.J));
        }

        public Vector3 GetMoveDirection()
        {
            float moveX = 0f;
            float moveZ = 0f;



            if (Input.GetKey(MappedKey(InputAction.R))) moveX += 1;
            if (Input.GetKey(MappedKey(InputAction.L))) moveX -= 1;

            if (Input.GetKey(MappedKey(InputAction.F))) moveZ += 1;
            if (Input.GetKey(MappedKey(InputAction.B))) moveZ -= 1;

            Vector3 D = new Vector3(moveX, 0f, moveZ);

            return D.normalized;//my dick angle lol ;)
        }

        public bool GetSprint()
        {
            return Input.GetKey(MappedKey(InputAction.S));
        }

        public int GetHotBar()
        {
            if (Input.GetKeyDown(MappedKey(InputAction.slot1))) return 0;
            if (Input.GetKeyDown(MappedKey(InputAction.slot2))) return 1;
            if (Input.GetKeyDown(MappedKey(InputAction.slot3))) return 2;
            return -1;
        }

        public bool GetInteract()
        {
            return Input.GetKeyDown(MappedKey(InputAction.interact));
        }
        public bool GetInteractHeld()
        {
            return Input.GetKey(MappedKey(InputAction.interact));
        }
        public Vector2 GetLookDelta()
        {
            float mY = Input.GetAxis("Mouse Y");
            float mX = Input.GetAxis("Mouse X");

            return new Vector2(mX, mY);
        }
        public bool GetDropInput()
        {
            return Input.GetKeyDown(MappedKey(InputAction.drop));
        }

        public bool GetFlashLight()
        {
            return Input.GetKeyDown(MappedKey(InputAction.light));
        }

        public bool GetPause()
        {
            return Input.GetKeyDown(MappedKey(InputAction.pause));
        }
    }

}