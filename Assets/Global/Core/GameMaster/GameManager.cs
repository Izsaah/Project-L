using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Monologue;
using UnityEngine;

namespace ProjectL.Global.Core.GameMaster
{
    [RequireComponent(typeof(WorldStateManager))]
    [RequireComponent(typeof(DroppedItemManager))]
    [RequireComponent(typeof(TimeManager))]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public WorldStateManager worldState { get; private set; }
        public DroppedItemManager droppedItems { get; private set; }
        public TimeManager timeManager { get; private set; }

        public MonologueUI monologueUI { get; private set; }

        public List<String> destroyObjects = new List<string>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                worldState = GetComponent<WorldStateManager>();
                droppedItems = GetComponent<DroppedItemManager>();
                timeManager = GetComponent<TimeManager>();

                monologueUI = FindAnyObjectByType<MonologueUI>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // public void markObjectAsdestroyed(string ID)
        // {
        //     if (!destroyObjects.Contains(ID))
        //     {
        //         destroyObjects.Add(ID);
        //         Debug.Log($"Game save state: {ID} is gone");
        //     }
        // }
        //    public bool isObjectDestroyed(string ID)
        //     {
        //         return destroyObjects.Contains(ID);
        //     }

    }

}
