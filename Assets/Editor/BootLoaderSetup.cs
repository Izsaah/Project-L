using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using ProjectL.Global.Scripts;
using System.Linq;

public class BootLoaderSetup
{
    [MenuItem("Tools/Setup Boot Loader")]
    public static void SetupBootLoader()
    {
        // 1. Open Map_1
        Scene map1 = EditorSceneManager.OpenScene("Assets/Scenes/Map/Map_1.unity", OpenSceneMode.Single);
        
        // Find GameMaster
        GameObject gameMaster = GameObject.Find("GameMaster");
        if (gameMaster == null)
        {
            Debug.LogError("Could not find GameMaster in Map_1!");
            return;
        }

        // Create new scene
        Scene bootScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
        bootScene.name = "00_BootLoader";

        // Move GameMaster to bootScene
        SceneManager.MoveGameObjectToScene(gameMaster, bootScene);

        // Add BootManager if not present
        if (gameMaster.GetComponent<BootManager>() == null)
        {
            BootManager bootManager = gameMaster.AddComponent<BootManager>();
            bootManager.nextSceneName = "MainMenu"; // Ensure it points to MainMenu
        }

        // Save Boot Scene
        string bootScenePath = "Assets/Scenes/00_BootLoader.unity";
        EditorSceneManager.SaveScene(bootScene, bootScenePath);

        // Save Map_1
        EditorSceneManager.SaveScene(map1);

        // Update Build Settings
        EditorBuildSettingsScene[] originalScenes = EditorBuildSettings.scenes;
        
        // Check if it already exists
        if (!originalScenes.Any(s => s.path == bootScenePath))
        {
            EditorBuildSettingsScene[] newScenes = new EditorBuildSettingsScene[originalScenes.Length + 1];
            
            newScenes[0] = new EditorBuildSettingsScene(bootScenePath, true);
            
            for(int i = 0; i < originalScenes.Length; i++)
            {
                newScenes[i+1] = originalScenes[i];
            }
            
            EditorBuildSettings.scenes = newScenes;
        }
        else
        {
            // Just move it to 0 if it exists
            var list = originalScenes.ToList();
            var bootSettings = list.First(s => s.path == bootScenePath);
            list.Remove(bootSettings);
            list.Insert(0, bootSettings);
            EditorBuildSettings.scenes = list.ToArray();
        }

        // Finally, unload Map_1 so only BootLoader is open
        EditorSceneManager.CloseScene(map1, true);

        Debug.Log("Boot Loader setup successfully! Build Settings updated.");
    }

    [MenuItem("Tools/Fix Cutscene Trigger Location")]
    public static void FixCutsceneTriggerLocation()
    {
        // Open Map_1
        Scene map1 = EditorSceneManager.OpenScene("Assets/Scenes/Map/Map_1.unity", OpenSceneMode.Single);

        bool changesMade = false;

        // 1. Find the rogue trigger on CutsceneImageManager and DESTROY it!
        GameObject uiManager = GameObject.Find("CutsceneImageManager");
        if (uiManager != null)
        {
            var rogueTrigger = uiManager.GetComponent<ProjectL.Local.MainMenu.Scripts.MenuUI.SceneCutsceneTrigger>();
            if (rogueTrigger != null)
            {
                Object.DestroyImmediate(rogueTrigger, true);
                Debug.Log("Removed rogue SceneCutsceneTrigger from CutsceneImageManager!");
                changesMade = true;
            }
        }

        // 2. Make sure Map1_AmbushTrigger is perfectly set up and ENABLED
        GameObject ambushObj = GameObject.Find("Map1_AmbushTrigger");
        if (ambushObj != null)
        {
            var trueTrigger = ambushObj.GetComponent<ProjectL.Local.MainMenu.Scripts.MenuUI.SceneCutsceneTrigger>();
            if (trueTrigger != null)
            {
                if (!trueTrigger.enabled)
                {
                    trueTrigger.enabled = true;
                    Debug.Log("Re-enabled Map1_AmbushTrigger!");
                    changesMade = true;
                }
                if (string.IsNullOrEmpty(trueTrigger.uniqueCutsceneID))
                {
                    trueTrigger.uniqueCutsceneID = "ambush";
                    Debug.Log("Set Map1_AmbushTrigger uniqueCutsceneID to 'ambush'!");
                    changesMade = true;
                }
            }
        }
        else
        {
            Debug.LogError("Could not find Map1_AmbushTrigger. Please run this tool again if you haven't yet.");
        }

        // Save if we fixed things
        if (changesMade)
        {
            EditorSceneManager.SaveScene(map1);
            Debug.Log("Fixed the rogue triggers in Map_1 and saved!");
        }
        else
        {
            Debug.Log("Everything already looks perfect in Map_1!");
        }
    }
}
