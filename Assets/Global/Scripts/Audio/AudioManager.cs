using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Script.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [Header("GLOBAL AUDIO SPEAKER")]
        [Tooltip("The speaker that PLAY FOR 2d UI, QUEST, SYSTEM SOUNDSSSSSSS")]
        [SerializeField] private AudioSource gES;

        private void Awake()
        {
            if (gES == null) gES = GetComponent<AudioSource>();

            gES.spatialBlend = 0f;
        }
        public void pSS(AudioData data)
        {
            if (data == null || data.aClip == null || data.aClip.Length == 0) Debug.LogWarning("GAP: no valida audio data or clip provided");

            int rI = Random.Range(0, data.aClip.Length);
            AudioClip CClip = data.aClip[rI];

            gES.PlayOneShot(CClip);

        }
    }
}