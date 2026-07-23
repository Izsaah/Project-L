using System.Collections.Generic;
using ProjectL.Global.Script.Inventory;
using UnityEngine;

namespace ProjectL.Global.Script.SaveSystem
{
    [CreateAssetMenu(fileName = "New ItemDatabase", menuName = "ProjectL/SaveSystem/ItemDatabase")]
    public class ItemDatabase : ScriptableObject
    {
        public List<ItemData> items = new List<ItemData>();

        public ItemData GetItemByName(string itemName)
        {
            foreach (ItemData item in items)
            {
                if (item != null && item.iN == itemName)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
