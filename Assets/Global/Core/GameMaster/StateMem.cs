using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Core.GameMaster
{

    public class StateMem : MonoBehaviour
    {
        [Header("REMEMRBER WHO YOU ARE")]
        [Tooltip("this must be unique name scenename_objectname_01 ex: Debug_crowbar_01, Debug_crowbar_02.")]
        public String ID;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                if (GameManager.Instance.worldState.isObjectDestroyed(ID))
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.Log("WHERE IS MASTER AHHHHHHHHHHHH");
            }
        }
        public void destroyNremember()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.worldState.MarkObjectAsDestroyed(ID);
            }
            Destroy(gameObject);
        }

    }

}
