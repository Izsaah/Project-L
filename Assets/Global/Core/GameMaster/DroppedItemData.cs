using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Inventory;
using UnityEngine;
namespace ProjectL.Global.Core.GameMaster
{
    [System.Serializable]
    public class DroppedItemData
    {
        public String dropId;
        public ItemData itemInf;
        public Vector3 dropPos;
        public string sceneName;

        [System.NonSerialized]
        public GameObject phy3Dmodel;
    }

}
