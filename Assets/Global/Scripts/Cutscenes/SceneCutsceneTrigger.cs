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

        [Header("Save System (Required for persistence)")]
        [Tooltip("A unique ID for this cutscene so the save system remembers it played (e.g. Map1_IntroCutscene)")]
        public string uniqueCutsceneID;

        private ProjectL.Global.Core.GameMaster.StateMem stateMem;
        private bool hasPlayed = false;

        private void Awake()
        {
            stateMem = GetComponent<ProjectL.Global.Core.GameMaster.StateMem>();

            // If we have a StateMem or a uniqueCutsceneID, check if it's already marked as destroyed/played
            if (ProjectL.Global.Core.GameMaster.GameManager.Instance != null)
            {
                string idToCheck = stateMem != null ? stateMem.ID : uniqueCutsceneID;
                if (!string.IsNullOrEmpty(idToCheck) && ProjectL.Global.Core.GameMaster.GameManager.Instance.worldState.isObjectDestroyed(idToCheck))
                {
                    Destroy(this); // Destroy only this script, not the whole GameObject (in case it's on the GameManager!)
                    return;
                }
            }
        }
        
        private IEnumerator Start()
        {
            if (ProjectL.Global.Core.GameMaster.GameManager.Instance != null)
            {
                string idToCheck = stateMem != null ? stateMem.ID : uniqueCutsceneID;
                if (!string.IsNullOrEmpty(idToCheck) && ProjectL.Global.Core.GameMaster.GameManager.Instance.worldState.isObjectDestroyed(idToCheck))
                {
                    Destroy(this);
                    yield break;
                }
            }

            if (hasPlayed) yield break;

            // Shut off player movement immediately so they can't run away
            if (playerH != null) playerH.enabled = false;
            if (playerM != null) playerM.enabled = false;

            // Wait for the UI manager to finish loading from the Master Scene
            while (CutSceneUIManager.Instance == null)
            {
                yield return null;
            }

            // Re-check memory right before playing! In case we were waiting a long time (e.g. from a Boot scene) 
            // and the save data was loaded while we were waiting!
            if (ProjectL.Global.Core.GameMaster.GameManager.Instance != null)
            {
                string idToCheck = stateMem != null ? stateMem.ID : uniqueCutsceneID;
                if (!string.IsNullOrEmpty(idToCheck) && ProjectL.Global.Core.GameMaster.GameManager.Instance.worldState.isObjectDestroyed(idToCheck))
                {
                    Destroy(this);
                    yield break;
                }
            }

            if (!hasPlayed)
            {
                PlayCutscene();
            }
        }

        private void PlayCutscene()
        {
            hasPlayed = true;
            
            // Subscribe to the finish event to restore player movement BEFORE destroying this script!
            CutSceneUIManager.Instance.Finish.AddListener(OnCutsceneFinished);
            
            CutSceneUIManager.Instance.ShowImage(cutsceneFile);

            if (stateMem != null)
            {
                // We cannot use stateMem.destroyNremember() because it destroys the gameObject.
                ProjectL.Global.Core.GameMaster.GameManager.Instance.worldState.MarkObjectAsDestroyed(stateMem.ID);
                Destroy(stateMem); // destroy the stateMem script immediately
            }
            else if (!string.IsNullOrEmpty(uniqueCutsceneID) && ProjectL.Global.Core.GameMaster.GameManager.Instance != null)
            {
                ProjectL.Global.Core.GameMaster.GameManager.Instance.worldState.MarkObjectAsDestroyed(uniqueCutsceneID);
            }
            else
            {
                Debug.LogWarning($"[SceneCutsceneTrigger] No StateMem or uniqueCutsceneID assigned on {gameObject.name}. This cutscene will play again when the game is loaded!");
            }
            
            // NOTE: We DO NOT call Destroy(this) here anymore! We wait for the cutscene to finish.
        }

        private void OnCutsceneFinished()
        {
            // Restore player movement!
            if (playerH != null) playerH.enabled = true;
            if (playerM != null) playerM.enabled = true;
            
            // Clean up the listener
            if (CutSceneUIManager.Instance != null)
            {
                CutSceneUIManager.Instance.Finish.RemoveListener(OnCutsceneFinished);
            }

            // NOW we destroy this trigger script so it's gone for good!
            Destroy(this);
        }
    }
}
