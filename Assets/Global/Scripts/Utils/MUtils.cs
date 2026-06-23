using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Movement;
using ProjectL.Scripts.Interface;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public static class MUtils
{
    public static float gCheck(CharacterController c, float currentY)
    {
        if (c.isGrounded && currentY < 0)
        {
            return -2f;
        }
        return currentY;
    }

    public static Vector3 CHM(Transform playerTransform, MovementSettings mS, IInputProvider iP, ref float c, ref bool exh, float dt, bool isFat)
    {
        float cS = mS.moveSpeed;

        Vector3 d = iP.GetMoveDirection();
        bool isMoving = d.sqrMagnitude > 0.01f;

        Vector3 realDirection = (playerTransform.right * d.x) + (playerTransform.forward * d.z);
        realDirection = realDirection.normalized;

        if (c <= 0f)
        {
            exh = true;
        }
        else if (c >= mS.ERT)
        {
            exh = false;
        }
        if (isFat)
        {
            cS = 2f;
            if (isMoving)
            {
                c -= 10f * dt;
            }
        }
        else if (iP.GetSprint() && c > 0f && isMoving && !exh)
        {
            cS = mS.sprintSpeed;
            c -= mS.drainRate * dt;

            Debug.Log("Draining Stamina! Current: " + c);
        }
        else
        {
            if (c < mS.maxStamina)
            {
                c += mS.restoreRate * dt;

                Debug.Log("Restoring Stamina... Current: " + c);
            }
        }
        c = Mathf.Clamp(c, 0f, mS.maxStamina);
        return realDirection * cS;
    }

    public static float CJAG(float currentY, MovementSettings mS, IInputProvider iP, CharacterController c)
    {
        if (iP.GetJump() && c.isGrounded)
        {
            return Mathf.Sqrt(mS.jumpHeight * -2f * mS.gravity);
        }
        return currentY + (mS.gravity * Time.deltaTime);
    }
}
