using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Script.CutScenes
{
    [CreateAssetMenu(fileName = "CutSceneDB", menuName = "ProjectL/CutSceneDB")]
    public class CutsceneDatabase : ScriptableObject
    {
        [System.Serializable]
        public struct CutsceneLine
        {
            [Tooltip("e.g., 'granny_saved'")]
            public string key;

            [Tooltip("Drag the actual 2d picture")]
            public Sprite[] imageSprite;
        }
        public List<CutsceneLine> db = new List<CutsceneLine>();
        public Sprite[] getImage(string key)
        {
            foreach (CutsceneLine line in db)
            {
                if (line.key == key)
                {
                    return line.imageSprite;
                }
            }
            Debug.LogWarning("carefull");
            return null;
        }

    }
}

