using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Core.GameMaster;
using UnityEngine;
namespace ProjectL.Global.Script.interaction
{
    public class LocalEndGameTrigger : MonoBehaviour
    {
        public void TellManagerToUpdate()
        {
            // This finds the manager through code instead of the Inspector
            MainEndManager manager = FindObjectOfType<MainEndManager>();
            if (manager != null)
            {
                manager.CheckNPCCount();
            }
        }
    }
}