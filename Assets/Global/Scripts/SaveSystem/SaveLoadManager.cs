using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.Player;
using ProjectL.Global.Script.Inventory;
using ProjectL.Global.Script.Movement;
using ProjectL.Scripts.Interface;
using ProjectL.Global.Script.interaction;

namespace ProjectL.Global.Script.SaveSystem
{
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance;

        public ItemDatabase itemDatabase;
        private string saveFilePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
            }
            else
            {
                Destroy(gameObject);
            }

            // Hook into scene loaded event to restore scene specific objects
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SaveGame()
        {
            SaveData data = new SaveData();
            data.currentScene = SceneManager.GetActiveScene().name;

            // 1. Save Player State (if player exists in current scene)
            PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
            if (player != null)
            {
                data.playerState.position = player.transform.position;
                data.playerState.rotation = player.transform.eulerAngles;

                PlayerInventory inventory = player.GetComponent<PlayerInventory>();
                if (inventory != null)
                {
                    data.playerState.activeSlotIndex = inventory.activeSlotIndex;
                    foreach (ItemData item in inventory.hotbarSlot)
                    {
                        if (item != null)
                            data.playerState.inventoryHotbar.Add(item.iN);
                        else
                            data.playerState.inventoryHotbar.Add("");
                    }
                }
            }

            // 2. Save WorldStateManager (Global state, IDs are globally unique)
            if (GameManager.Instance != null && GameManager.Instance.worldState != null)
            {
                WorldStateManager wsm = GameManager.Instance.worldState;
                data.globalState = new GlobalGameState(); // We can add global flags here

                // To fit the user's SceneSaveData architecture, we'll store global world state in a special "Global" scene entry for now
                // Or simply serialize them natively. Let's put them in a "Global" SceneSaveData entry.
                SceneSaveData globalSceneData = new SceneSaveData();
                globalSceneData.destroyedObjects = new List<string>(wsm.destroyedObjects);

                foreach (var kvp in wsm.objectProgress)
                {
                    globalSceneData.objectProgress.Add(new StringIntPair(kvp.Key, kvp.Value));
                }
                foreach (var kvp in wsm.sSP)
                {
                    globalSceneData.spawnPositions.Add(new StringIntPair(kvp.Key, kvp.Value));
                }

                data.sceneDataList.Add(new SceneSaveEntry { sceneName = "Global", sceneData = globalSceneData });
            }

            // 3. Save Dropped Items (split by sceneName)
            Dictionary<string, SceneSaveData> sceneDataDict = new Dictionary<string, SceneSaveData>();

            if (GameManager.Instance != null && GameManager.Instance.droppedItems != null)
            {
                foreach (DroppedItemData drop in GameManager.Instance.droppedItems.allDropItems)
                {
                    if (drop.itemInf == null || string.IsNullOrEmpty(drop.sceneName)) continue;

                    if (!sceneDataDict.ContainsKey(drop.sceneName))
                    {
                        sceneDataDict[drop.sceneName] = new SceneSaveData();
                    }

                    DroppedItemSaveData dropData = new DroppedItemSaveData
                    {
                        dropId = drop.dropId,
                        itemName = drop.itemInf.iN,
                        dropPos = drop.dropPos
                    };
                    sceneDataDict[drop.sceneName].droppedItems.Add(dropData);
                }
            }

            // 4. Save Custom SceneSaveables
            SceneStateManager sceneState = FindAnyObjectByType<SceneStateManager>();
            if (sceneState != null)
            {
                string sName = SceneManager.GetActiveScene().name;
                if (!sceneDataDict.ContainsKey(sName))
                {
                    sceneDataDict[sName] = new SceneSaveData();
                }

                Dictionary<string, string> state = sceneState.CollectSceneState();
                foreach (var kvp in state)
                {
                    sceneDataDict[sName].customSaveables.Add(new StringStringPair(kvp.Key, kvp.Value));
                }
            }

            foreach (var kvp in sceneDataDict)
            {
                data.sceneDataList.Add(new SceneSaveEntry { sceneName = kvp.Key, sceneData = kvp.Value });
            }

            // Serialize and Save
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game Saved to: " + saveFilePath);
        }

        private SaveData activeSaveSession = null;

        public void LoadGame()
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.LogWarning("No save file found at " + saveFilePath);
                return;
            }

            string json = File.ReadAllText(saveFilePath);
            activeSaveSession = JsonUtility.FromJson<SaveData>(json);

            // 1. Try to Restore WorldStateManager (Global) right away if GameManager exists
            TryRestoreGlobalState(activeSaveSession);

            // 2. Load the Main Scene
            if (!string.IsNullOrEmpty(activeSaveSession.currentScene) && SceneManager.GetActiveScene().name != activeSaveSession.currentScene)
            {
                SceneManager.LoadScene(activeSaveSession.currentScene);
                // The rest of the state (Player, DroppedItems, Additive Chunks) will be restored in OnSceneLoaded
            }
            else
            {
                // Already in the correct scene, restore immediately
                RestoreSceneState(activeSaveSession, SceneManager.GetActiveScene().name);
                // For additive chunks that are already loaded, we would technically need to iterate over them, 
                // but usually LoadGame is called from a clean state (like MainMenu).
            }
        }

        private void TryRestoreGlobalState(SaveData data)
        {
            SceneSaveEntry globalEntry = data.sceneDataList.Find(x => x.sceneName == "Global");
            if (globalEntry != null && GameManager.Instance != null && GameManager.Instance.worldState != null)
            {
                WorldStateManager wsm = GameManager.Instance.worldState;
                wsm.destroyedObjects = new List<string>(globalEntry.sceneData.destroyedObjects);

                wsm.objectProgress.Clear();
                foreach (StringIntPair pair in globalEntry.sceneData.objectProgress)
                {
                    wsm.objectProgress[pair.key] = pair.value;
                }

                wsm.sSP.Clear();
                foreach (StringIntPair pair in globalEntry.sceneData.spawnPositions)
                {
                    wsm.sSP[pair.key] = pair.value;
                }

                // Remove it so we don't restore it again on every chunk load
                data.sceneDataList.Remove(globalEntry);
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Only restore from file if we are currently running an active save session!
            if (activeSaveSession != null)
            {
                // In case GameManager didn't exist when LoadGame was called (e.g. from Main Menu), try restoring global state now!
                TryRestoreGlobalState(activeSaveSession);
                
                // Restore the specific data for this scene (whether it's the main scene or an additive chunk)
                RestoreSceneState(activeSaveSession, scene.name);

                // We DO NOT set activeSaveSession to null here! We keep it alive in memory
                // so that when SceneChunkLoader streams in "Map_1.H2" later, it will also get restored.
                // The RestoreSceneState method will consume/remove the scene's data so it only happens once.
            }
        }

        private void RestoreSceneState(SaveData data, string sceneName)
        {
            SceneSaveEntry sceneEntry = data.sceneDataList.Find(x => x.sceneName == sceneName);

            // Restore Player State
            if (data.currentScene == sceneName) // Only restore player if this is the active saved scene
            {
                PlayerMovement player = FindAnyObjectByType<PlayerMovement>();
                if (player != null)
                {
                    // Disable character controller before teleporting
                    CharacterController cc = player.GetComponent<CharacterController>();
                    if (cc != null) cc.enabled = false;

                    player.transform.position = data.playerState.position;
                    player.transform.eulerAngles = data.playerState.rotation;

                    if (cc != null) cc.enabled = true;

                    // Restore Inventory
                    PlayerInventory inventory = player.GetComponent<PlayerInventory>();
                    if (inventory != null && itemDatabase != null)
                    {
                        for (int i = 0; i < inventory.hotbarSlot.Length; i++)
                        {
                            inventory.hotbarSlot[i] = null;
                        }

                        for (int i = 0; i < data.playerState.inventoryHotbar.Count && i < inventory.hotbarSlot.Length; i++)
                        {
                            string itemName = data.playerState.inventoryHotbar[i];
                            if (!string.IsNullOrEmpty(itemName))
                            {
                                ItemData item = itemDatabase.GetItemByName(itemName);
                                inventory.hotbarSlot[i] = item;
                            }
                        }
                        inventory.activeSlotIndex = data.playerState.activeSlotIndex;
                        // Tell inventory to refresh visuals
                        inventory.SendMessage("HandleSlotSelection", SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            // Restore Dropped Items for this scene
            if (GameManager.Instance != null && GameManager.Instance.droppedItems != null)
            {
                DroppedItemManager dim = GameManager.Instance.droppedItems;

                // Clear existing dropped items in the manager that belong to this scene (to prevent duplicates)
                dim.allDropItems.RemoveAll(d => d.sceneName == sceneName);

                // Find existing dropped items in the scene and destroy them
                ItemPickup[] existingPickups = FindObjectsByType<ItemPickup>(FindObjectsSortMode.None);
                foreach (var pickup in existingPickups)
                {
                    Destroy(pickup.gameObject);
                }

                // Spawn saved dropped items
                if (sceneEntry != null && itemDatabase != null)
                {
                    foreach (DroppedItemSaveData dropData in sceneEntry.sceneData.droppedItems)
                    {
                        ItemData item = itemDatabase.GetItemByName(dropData.itemName);
                        if (item != null && item.fab != null)
                        {
                            GameObject model = Instantiate(item.fab, dropData.dropPos, Quaternion.identity);
                            dim.allDropItems.Add(new DroppedItemData
                            {
                                dropId = dropData.dropId,
                                itemInf = item,
                                dropPos = dropData.dropPos,
                                sceneName = sceneName,
                                phy3Dmodel = model
                            });

                            ItemPickup pickup = model.GetComponent<ItemPickup>();
                            if (pickup != null)
                            {
                                pickup.runtimeDropID = dropData.dropId;
                            }
                        }
                    }
                }
            }

            // Restore Custom SceneSaveables
            SceneStateManager sceneState = FindAnyObjectByType<SceneStateManager>();
            if (sceneState != null && sceneEntry != null)
            {
                Dictionary<string, string> state = new Dictionary<string, string>();
                foreach (StringStringPair pair in sceneEntry.sceneData.customSaveables)
                {
                    state[pair.key] = pair.value;
                }
                sceneState.RestoreSceneState(state);
            }

            // Remove the scene entry so if we unload and reload this chunk, we don't restore old save data again!
            if (sceneEntry != null)
            {
                data.sceneDataList.Remove(sceneEntry);
            }

            // StateMem objects (doors, triggers) will automatically check GameManager.Instance.worldState on Start()
            // and destroy themselves if they are in the destroyedObjects list.
            // We just need to make sure StateMem runs AFTER GameManager is restored. 
            // Since SaveLoadManager loads worldState before SceneManager.LoadScene, StateMem's Start() will read the correct state!
        }
    }
}
