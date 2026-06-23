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


        private void Start()
        {
            if (playerH != null) playerH.enabled = false;
            if (playerM != null) playerM.enabled = false;

            if (cutsceneManager != null)
            {
                cutsceneManager.ShowImage(CutSceneID);
            }
            else
            {
                Debug.LogWarning("FAILEEEEEDDDDDDDDDDDDd");
            }
        }
    }
}
