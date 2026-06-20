using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Script.Monologue
{
    [CreateAssetMenu(fileName = "LanguageDB", menuName = "ProjectL/Language DB")]
    public class TextDatabase : ScriptableObject
    {
        [System.Serializable]
        public struct TLline
        {
            [Tooltip("The ID you type in your code (e.g., 'thought_heavy_item')")]
            public string key;
            [TextArea(2, 5)]
            public string tText;


        }
        public List<TLline> db = new List<TLline>();
        public string GetText(String searchKey)
        {
            foreach (TLline line in db)
            {
                if (line.key == searchKey)
                {
                    return line.tText;
                }
            }
            Debug.LogWarning($"this could be fck {searchKey}");
            return searchKey;
        }
    }
}