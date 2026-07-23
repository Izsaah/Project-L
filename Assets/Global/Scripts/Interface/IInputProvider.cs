using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectL.Scripts.Interface
{
    public interface IInputProvider
    {
        Vector3 GetMoveDirection(); // direction gaming lol
        bool GetJump();//jump = true or false.
        bool GetSprint();//run mother fuck run
        int GetHotBar();//hotbar
        bool GetInteract();//touch me i dare yah ;)
        bool GetInteractHeld();//TOUCH ME HARDER
        bool GetDropInput();//angry bird moment lol
        Vector2 GetLookDelta();//spicy stuff kinkky : ) 
        bool GetFlashLight();
        bool GetPause();
    }

}
