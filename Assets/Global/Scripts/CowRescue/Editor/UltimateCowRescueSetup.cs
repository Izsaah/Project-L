using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using ProjectL.Global.Script.CowRescue;

public class UltimateCowRescueSetup : EditorWindow
{
    [MenuItem("Project-L/1-CLICK SETUP COW RESCUE (NEW)")]
    public static void SetupEverything()
    {
        // 1. Create Canvas
        GameObject canvasGO = GameObject.Find("Minigame_PullRope");
        if (canvasGO == null)
        {
            canvasGO = new GameObject("Minigame_PullRope");
            Canvas canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();
            
            GameObject panelGO = new GameObject("MinigamePanel");
            panelGO.transform.SetParent(canvasGO.transform, false);
            Image panelImage = panelGO.AddComponent<Image>();
            panelImage.color = new Color(0, 0, 0, 0.5f);
            RectTransform panelRect = panelGO.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.1f);
            panelRect.anchorMax = new Vector2(0.5f, 0.1f);
            panelRect.sizeDelta = new Vector2(600, 150);
            
            GameObject sliderGO = DefaultControls.CreateSlider(new DefaultControls.Resources());
            sliderGO.name = "ProgressSlider";
            sliderGO.transform.SetParent(panelGO.transform, false);
            Slider slider = sliderGO.GetComponent<Slider>();
            slider.interactable = false;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            RectTransform sliderRect = sliderGO.GetComponent<RectTransform>();
            sliderRect.anchorMin = new Vector2(0.5f, 0.5f);
            sliderRect.anchorMax = new Vector2(0.5f, 0.5f);
            sliderRect.sizeDelta = new Vector2(500, 20);
            sliderRect.anchoredPosition = new Vector2(0, -20);
            
            GameObject targetZoneGO = new GameObject("TargetZoneUI");
            targetZoneGO.transform.SetParent(sliderRect, false);
            Image targetZoneImage = targetZoneGO.AddComponent<Image>();
            targetZoneImage.color = new Color(0, 1, 0, 0.5f);
            RectTransform targetZoneRect = targetZoneGO.GetComponent<RectTransform>();
            targetZoneRect.anchorMin = new Vector2(0f, 0.5f);
            targetZoneRect.anchorMax = new Vector2(0f, 0.5f);
            targetZoneRect.sizeDelta = new Vector2(50, 40);
            targetZoneRect.anchoredPosition = new Vector2(250, 0);
            
            GameObject statusTextGO = new GameObject("StatusText");
            statusTextGO.transform.SetParent(panelGO.transform, false);
            TextMeshProUGUI statusText = statusTextGO.AddComponent<TextMeshProUGUI>();
            statusText.text = "Press SPACE to pull!";
            statusText.fontSize = 32;
            statusText.alignment = TextAlignmentOptions.Center;
            statusText.color = Color.white;
            RectTransform textRect = statusTextGO.GetComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.5f, 1f);
            textRect.anchorMax = new Vector2(0.5f, 1f);
            textRect.sizeDelta = new Vector2(500, 50);
            textRect.anchoredPosition = new Vector2(0, -30);
            
            PullRopeMinigame minigameScript = canvasGO.AddComponent<PullRopeMinigame>();
            SerializedObject minigameSO = new SerializedObject(minigameScript);
            minigameSO.FindProperty("minigamePanel").objectReferenceValue = panelGO;
            minigameSO.FindProperty("progressSlider").objectReferenceValue = slider;
            minigameSO.FindProperty("targetZoneUI").objectReferenceValue = targetZoneRect;
            minigameSO.FindProperty("statusText").objectReferenceValue = statusText;
            minigameSO.ApplyModifiedProperties();
            
            panelGO.SetActive(false);
            Undo.RegisterCreatedObjectUndo(canvasGO, "Setup UI");
        }

        // 2. Create Manager
        GameObject managerGO = GameObject.Find("CowRescueManager");
        CowRescueManager managerScript = null;
        if (managerGO == null)
        {
            managerGO = new GameObject("CowRescueManager");
            managerScript = managerGO.AddComponent<CowRescueManager>();
            Undo.RegisterCreatedObjectUndo(managerGO, "Setup Manager");
        }
        else
        {
            managerScript = managerGO.GetComponent<CowRescueManager>();
        }

        SerializedObject managerSO = new SerializedObject(managerScript);
        managerSO.FindProperty("qteMinigame").objectReferenceValue = canvasGO.GetComponent<PullRopeMinigame>();

        // Find Player position to spawn things nearby
        Vector3 spawnPos = new Vector3(0, 0, 0);
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) player = GameObject.Find("Player");
        if (player != null) spawnPos = player.transform.position;

        // 3. Create Safe Zone
        GameObject safeZoneGO = GameObject.Find("SafeZoneTrigger_Placeholder");
        if (safeZoneGO == null)
        {
            safeZoneGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            safeZoneGO.name = "SafeZoneTrigger_Placeholder";
            safeZoneGO.transform.position = spawnPos + new Vector3(-3, 0, 3);
            BoxCollider boxCol = safeZoneGO.GetComponent<BoxCollider>();
            boxCol.isTrigger = true;
            safeZoneGO.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 1, 0.3f);
            safeZoneGO.AddComponent<SafeZoneTrigger>();
            Undo.RegisterCreatedObjectUndo(safeZoneGO, "Setup Safe Zone");
        }

        // 4. Create Flood Water
        GameObject floodGO = GameObject.Find("FloodWater");
        if (floodGO == null)
        {
            floodGO = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floodGO.name = "FloodWater";
            floodGO.transform.position = spawnPos + new Vector3(0, -5, 0);
            floodGO.transform.localScale = new Vector3(50, 10, 50);
            Material floodMat = new Material(Shader.Find("Standard"));
            floodMat.color = new Color(0, 0, 1, 0.5f);
            floodMat.SetFloat("_Mode", 3);
            floodMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            floodMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            floodMat.SetInt("_ZWrite", 0);
            floodMat.DisableKeyword("_ALPHATEST_ON");
            floodMat.EnableKeyword("_ALPHABLEND_ON");
            floodMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            floodMat.renderQueue = 3000;
            floodGO.GetComponent<MeshRenderer>().material = floodMat;
            
            FloodController floodScript = floodGO.AddComponent<FloodController>();
            floodScript.autoStart = true;
            floodGO.SetActive(false);
            Undo.RegisterCreatedObjectUndo(floodGO, "Setup Flood Water");
        }
        managerSO.FindProperty("floodWaterObject").objectReferenceValue = floodGO;

        // 5. Create Carry Trigger
        GameObject carryGO = GameObject.Find("CarryTrigger");
        if (carryGO == null)
        {
            carryGO = new GameObject("CarryTrigger");
            carryGO.transform.position = spawnPos + new Vector3(0, 0, 2);
            BoxCollider carryCol = carryGO.AddComponent<BoxCollider>();
            carryCol.isTrigger = true;
            carryCol.size = new Vector3(3, 3, 3);
            carryGO.AddComponent<RescueNPCInteract>();
            Undo.RegisterCreatedObjectUndo(carryGO, "Setup Carry Trigger");
        }
        managerSO.FindProperty("carryTriggerObject").objectReferenceValue = carryGO;

        // 6. Assign HEAVY item
        var heavyItem = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Global/Data/Item/HEAVY.asset");
        if (heavyItem != null)
        {
            managerSO.FindProperty("injuredNpcItemData").objectReferenceValue = heavyItem;
        }

        managerSO.ApplyModifiedProperties();

        // 7. Create Start Minigame Trigger
        GameObject startTrigger = GameObject.Find("StartMinigameTrigger");
        if (startTrigger == null)
        {
            startTrigger = new GameObject("StartMinigameTrigger");
            startTrigger.transform.position = spawnPos + new Vector3(2, 0, 0);
            BoxCollider boxCol = startTrigger.AddComponent<BoxCollider>();
            boxCol.isTrigger = true;
            boxCol.size = new Vector3(2, 2, 2);
            startTrigger.AddComponent<StartRescueInteract>();
            Undo.RegisterCreatedObjectUndo(startTrigger, "Create StartMinigameTrigger");
        }

        Debug.Log("🎉 ĐÃ TỰ ĐỘNG CÀI ĐẶT TOÀN BỘ MINIGAME THÀNH CÔNG! 🎉");
    }
}
