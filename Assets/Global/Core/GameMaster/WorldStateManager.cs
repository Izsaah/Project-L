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
        //memorey
        public Dictionary<string, int> objectProgress = new Dictionary<string, int>();

        public Dictionary<string, int> sSP = new Dictionary<string, int>();


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

        public void SaveProgress(string id, int pV)
        {
            if (objectProgress.ContainsKey(id))
            {
                objectProgress[id] = pV;
            }
            else
            {
                objectProgress.Add(id, pV);
            }
            Debug.Log("save");

        }
        public int GetOrSetSpawnIndex(string id, int TP)
        {
            if (sSP.ContainsKey(id))
            {
                return sSP[id];
            }
            int nRI = Random.Range(0, TP);
            sSP.Add(id, nRI);
            return nRI;
        }
        public int GetProgress(string id, int dV)
        {
            if (objectProgress.ContainsKey(id))
            {
                return objectProgress[id];
            }
            return dV;
        }
    }
}