using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO.Enumeration;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
namespace ProjectL.Global.Script.Inventory
{
    public enum IS
    {
        s = 1, m = 2, h = 3
    }
    [CreateAssetMenu(fileName = "New Item", menuName = "ProjectL/Item")]
    public class ItemData : ScriptableObject
    {
        public string iN;
        public Sprite i;
        //size
        [Header("Mechanics")]
        public IS size = IS.s;
        [Header("Visuals")]
        public GameObject fab;// fapping =)))))
        public Vector3 rotOS;
    }
}

