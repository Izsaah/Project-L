using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Core.GameMaster
{
    public class WorldStateManager : MonoBehaviour
    {
        [Header("WORLDDDDDDDDDDDDDDDDDDDDDD SAVEEEEEEEEEEE")]
        [Tooltip("List of IDS for solced puzzles and picked-up items")]
        public List<string> destroyedObjects = new List<string>();

        public void MarkObjectAsDestroyed(string ID)
        {
            if (!destroyedObjects.Contains(ID))
            {
                destroyedObjects.Add(ID);
                Debug.Log($"WorldState: {ID} is gone");
            }
        }
        public bool isObjectDestroyed(string ID)
        {
            return destroyedObjects.Contains(ID);
        }
    }
}