using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.CutScenes;
using UnityEngine;

namespace ProjectL.Local.MainMenu.Scripts.MenuUI
{
    public class SceneCutsceneTrigger : MonoBehaviour
    {
        [Header("Ambush UI")]
        public CutSceneUIManager cutsceneManager;

        [Header("What is the first image?")]
        [Tooltip("Drag the Cutscene file here!")]
        public CutsceneDatabase cutsceneFile; // CHANGED FROM STRING TO FILE

        [Header("LOCK THE FUCK DOWN MF YOU STUPID ASS PLAYER")]
        [Tooltip("FUCK YOU MOVEMNT")]
        public MonoBehaviour playerM;
        [Tooltip("FUCK YOU HEADER")]
        public MonoBehaviour playerH;

        private void Awake()
        {
            if (CutSceneUIManager.Instance != null)
            {
                CutSceneUIManager.Instance.ShowImage(cutsceneFile);
            }
        }
        
        private IEnumerator Start()
        {
            // Shut off player movement immediately so they can't run away
            if (playerH != null) playerH.enabled = false;
            if (playerM != null) playerM.enabled = false;

            // Wait for the UI manager to finish loading from the Master Scene
            while (CutSceneUIManager.Instance == null)
            {
                yield return null;
            }

            // Now trigger the cutscene using the file
            CutSceneUIManager.Instance.ShowImage(cutsceneFile);
        }
    }
}
