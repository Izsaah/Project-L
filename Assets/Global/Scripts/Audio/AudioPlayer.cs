using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ProjectL.Global.Script.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [Header("Audio Components")]
        [Tooltip("The speaker that will actually play the sound.")]
        [SerializeField] private AudioSource aS;
        [Header("Pitch rules")]
        [SerializeField] private float miP = 0.9f;
        [SerializeField] private float maP = 1.1f;
        public void PlaySWR(AudioData data)
        {
            if (data == null || data.aClip == null || data.aClip.Length == 0)
            {
                Debug.LogWarning("INVALID CARE FULLLLLLLLLl");
                return;
            }
            int rI = Random.Range(0, data.aClip.Length);
            AudioClip cClip = data.aClip[rI];

            aS.pitch = Random.Range(miP, maP);
            aS.PlayOneShot(cClip);
        }
    }
}