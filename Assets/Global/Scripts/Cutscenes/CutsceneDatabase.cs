using UnityEngine;

namespace ProjectL.Global.Script.CutScenes
{
    [CreateAssetMenu(fileName = "New Cutscene", menuName = "ProjectL/CutScene")]
    public class CutsceneDatabase : ScriptableObject
    {
        [Tooltip("Drag the actual 2D pictures for THIS specific cutscene here")]
        public Sprite[] images;

        [Header("Audio")]
        [Tooltip("OPTIONAL: Drag AudioData here to play when the matching image appears. Element 0 plays when Image 0 appears.")]
        public ProjectL.Global.Script.Audio.AudioData[] sounds;
    }
}
