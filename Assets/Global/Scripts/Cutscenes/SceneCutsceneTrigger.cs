using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.CutScenes;

using UnityEngine;
namespace ProjectL.Local.MainMenu.Scripts.MenuUI
{
    public class SceneCutsceneTrigger : MonoBehaviour
    {
        [Header("Amubush UI")]
        public CutSceneUIManager cutsceneManager;

        [Header("What is the first image?")]
        [Tooltip("Type the cutscene Key here=)))))))))))")]
        public string CutSceneID = "";
        [Header("LOCK THE FUCK DOWN MF YOU STUPID ASS PLAYER")]
        [Tooltip("FUCK YOU MOVEMNT")]
        public MonoBehaviour playerM;
        [Tooltip("FUCK YOU HEADER")]
        public MonoBehaviour playerH;

        // this will not break i swear to god =))))))))) bruh bruh lmao
        private void Awake()
        {
            if (CutSceneUIManager.Instance != null)
            {
                CutSceneUIManager.Instance.ShowImage(CutSceneID);
            }
        }
        // please don't break ;-;
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

            // Now trigger the cutscene
            CutSceneUIManager.Instance.ShowImage(CutSceneID);
        }
    }
}
