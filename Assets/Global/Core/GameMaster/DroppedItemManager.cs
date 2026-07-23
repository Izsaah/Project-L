using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectL.Global.Core.GameMaster
{
    public class DroppedItemManager : MonoBehaviour
    {

        [Header("REMRMEBER  WHOOOOOO ARE YOUUUUUUUU")]
        public List<DroppedItemData> allDropItems = new List<DroppedItemData>();

        public String RND(ItemData data, Vector3 pos, GameObject model)
        {
            string ID = System.Guid.NewGuid().ToString();
            string currentScene = SceneManager.GetActiveScene().name;

            allDropItems.Add(new DroppedItemData { dropId = ID, itemInf = data, dropPos = pos, sceneName = currentScene, phy3Dmodel = model });


            return ID;
        }

        public void RemoveDroppedItem(String ID)
        {
            allDropItems.RemoveAll(item => item.dropId == ID);
        }
    }

}
