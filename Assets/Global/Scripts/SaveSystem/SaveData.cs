using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectL.Global.Script.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        public string saveVersion = "1.0";
        public string currentScene;

        public PlayerSaveData playerState = new PlayerSaveData();
        public GlobalGameState globalState = new GlobalGameState();

        // JsonUtility doesn't serialize Dictionaries natively, so we use a List of a custom struct to map Scene Name -> SceneSaveData
        public List<SceneSaveEntry> sceneDataList = new List<SceneSaveEntry>();
    }

    [Serializable]
    public class PlayerSaveData
    {
        public Vector3 position;
        public Vector3 rotation;
        public List<string> inventoryHotbar = new List<string>();
        public int activeSlotIndex;
    }

    [Serializable]
    public class GlobalGameState
    {
        // Add global flags, completed objectives, etc. here
    }

    [Serializable]
    public class SceneSaveEntry
    {
        public string sceneName;
        public SceneSaveData sceneData;
    }

    [Serializable]
    public class SceneSaveData
    {
        public List<string> destroyedObjects = new List<string>();
        
        public List<StringIntPair> objectProgress = new List<StringIntPair>();
        public List<StringIntPair> spawnPositions = new List<StringIntPair>();

        public List<DroppedItemSaveData> droppedItems = new List<DroppedItemSaveData>();
        
        public List<StringStringPair> customSaveables = new List<StringStringPair>();
    }

    [Serializable]
    public struct StringIntPair
    {
        public string key;
        public int value;

        public StringIntPair(string k, int v)
        {
            key = k;
            value = v;
        }
    }

    [Serializable]
    public struct StringStringPair
    {
        public string key;
        public string value;

        public StringStringPair(string k, string v)
        {
            key = k;
            value = v;
        }
    }

    [Serializable]
    public class DroppedItemSaveData
    {
        public string dropId;
        public string itemName;
        public Vector3 dropPos;
    }
}
