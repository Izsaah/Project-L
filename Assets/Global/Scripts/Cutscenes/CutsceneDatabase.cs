using UnityEngine;

namespace ProjectL.Global.Script.CutScenes
{
    // You keep the same script name, but now it builds individual cutscenes!
    [CreateAssetMenu(fileName = "New Cutscene", menuName = "ProjectL/CutScene")]
    public class CutsceneDatabase : ScriptableObject
    {
        [Tooltip("Drag the actual 2D pictures for THIS specific cutscene here")]
        public Sprite[] images;
    }
}
