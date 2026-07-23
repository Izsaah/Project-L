using System.Collections.Generic;
using UnityEngine;

namespace ProjectL.Global.Script.SaveSystem
{
    public interface ISaveable
    {
        string GetUniqueId();
        string SaveState();
        void LoadState(string stateData);
    }

    public class SceneStateManager : MonoBehaviour
    {
        // This manager can be placed in any scene to track ISaveable objects that aren't natively supported by WorldStateManager.
        private List<ISaveable> saveableObjects = new List<ISaveable>();

        private void Awake()
        {
            // Find all ISaveable objects in the scene
            var allMonoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var mono in allMonoBehaviours)
            {
                if (mono is ISaveable saveable)
                {
                    saveableObjects.Add(saveable);
                }
            }
        }

        public Dictionary<string, string> CollectSceneState()
        {
            Dictionary<string, string> state = new Dictionary<string, string>();
            foreach (var saveable in saveableObjects)
            {
                state[saveable.GetUniqueId()] = saveable.SaveState();
            }
            return state;
        }

        public void RestoreSceneState(Dictionary<string, string> state)
        {
            foreach (var saveable in saveableObjects)
            {
                if (state.ContainsKey(saveable.GetUniqueId()))
                {
                    saveable.LoadState(state[saveable.GetUniqueId()]);
                }
            }
        }
    }
}
