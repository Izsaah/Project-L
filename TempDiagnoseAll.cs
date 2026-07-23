using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using TMPro;

public class TempDiagnoseAll
{
    public static void Execute()
    {
        string h5Path = "Assets/Scenes/Map/Map_1/Map_1.H5.unity";
        Scene h5Scene = SceneManager.GetSceneByPath(h5Path);
        if (!h5Scene.isLoaded) h5Scene = EditorSceneManager.OpenScene(h5Path, OpenSceneMode.Additive);

        // ========== 1. DialogueUIManager ==========
        var dm = Object.FindObjectOfType<ProjectL.Global.Script.Dialogue.DialogueUIManager>(true);
        if (dm != null)
        {
            Debug.Log($"[DM] Found DialogueUIManager on: {dm.gameObject.name}");
            Debug.Log($"[DM] dialogueBox: {(dm.dialogueBox != null ? dm.dialogueBox.name : "NULL")}");
            Debug.Log($"[DM] speakerNameText: {(dm.speakerNameText != null ? dm.speakerNameText.name : "NULL")}");
            Debug.Log($"[DM] dBT (body): {(dm.dBT != null ? dm.dBT.name : "NULL")}");
            if (dm.dBT != null)
            {
                Debug.Log($"[DM] dBT isActiveAndEnabled: {dm.dBT.isActiveAndEnabled}, alpha: {dm.dBT.alpha}, enableWordWrapping: {dm.dBT.enableWordWrapping}, overflow: {dm.dBT.overflowMode}");
            }
            if (dm.speakerNameText != null)
            {
                Debug.Log($"[DM] speakerNameText isActiveAndEnabled: {dm.speakerNameText.isActiveAndEnabled}");
            }
        }
        else Debug.Log("[DM] DialogueUIManager NOT FOUND");

        // ========== 2. Long_mong_ga ==========
        GameObject npc = GameObject.Find("Long_mong_ga");
        if (npc != null)
        {
            Debug.Log($"[NPC] Found at path: {GetPath(npc.transform)}");
            Debug.Log($"[NPC] Active: {npc.activeSelf}");

            var dt = npc.GetComponent<ProjectL.Global.Script.Dialogue.DialogueTrigger>();
            Debug.Log($"[NPC] DialogueTrigger: {(dt != null ? "FOUND, enabled=" + dt.enabled : "NOT FOUND")}");

            var rnpc = npc.GetComponent<ProjectL.Global.Script.CowRescue.RescueNPCInteract>();
            Debug.Log($"[NPC] RescueNPCInteract: {(rnpc != null ? "FOUND, enabled=" + rnpc.enabled : "NOT FOUND")}");

            var colliders = npc.GetComponents<Collider>();
            foreach (var c in colliders)
            {
                Debug.Log($"[NPC] Collider: {c.GetType().Name}, isTrigger={c.isTrigger}, enabled={c.enabled}");
            }
            
            var dad = npc.GetComponent<DisableAfterDialogue>();
            Debug.Log($"[NPC] DisableAfterDialogue: {(dad != null ? "FOUND" : "NOT FOUND")}");
        }
        else Debug.Log("[NPC] Long_mong_ga NOT FOUND");

        // ========== 3. CarryTrigger ==========
        GameObject[] allObjs = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (var go in allObjs)
        {
            if (go.name == "CarryTrigger")
            {
                Debug.Log($"[CT] CarryTrigger found: scene={go.scene.name}, active={go.activeSelf}, parent={go.transform.parent?.name}");
                var rnpc2 = go.GetComponent<ProjectL.Global.Script.CowRescue.RescueNPCInteract>();
                Debug.Log($"[CT] RescueNPCInteract: {(rnpc2 != null ? "FOUND" : "NOT FOUND")}");
            }
        }
    }
    
    static string GetPath(Transform t)
    {
        if (t.parent == null) return t.name;
        return GetPath(t.parent) + "/" + t.name;
    }
}
