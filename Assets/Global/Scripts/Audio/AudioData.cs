using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
namespace ProjectL.Global.Script.Audio
{
    [CreateAssetMenu(fileName = "NewAudios", menuName = "ProjectL/Audis")]
    public class AudioData : ScriptableObject
    {
        [Tooltip("TAG EM BOY TAG EM")]
        public string nTag;
        [Tooltip("WHAT YAH HEARING BOY WHAT YAH HEARING")]
        public AudioClip[] aClip;
    }
}
