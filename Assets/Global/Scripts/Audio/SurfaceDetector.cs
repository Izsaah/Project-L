using System;
using System.Collections;
using System.Collections.Generic;
using ProjectL.Global.Script.Audio;
using UnityEngine;
namespace ProjectL.Global.Script.Audio
{
    public class SurfaceDetector : MonoBehaviour
    {
        [Header("Core References")]
        [Tooltip("The AudioPlayer component that acts as the speaker.")]
        [SerializeField] private AudioPlayer audioPlayer;

        [Header("Raycast Settings")]
        [Tooltip("The physical point where the surface check originates (e.g., feet, tires, base).")]
        [SerializeField] private Transform checkOrigin;
        [SerializeField] private float raycastDistance = 0.5f;
        [SerializeField] private LayerMask groundLayer;

        [Header("Surface Mapping")]
        [Tooltip("Drop all your AudioData assets here (Wood, Concrete, etc).")]
        [SerializeField] private AudioData[] surfaceDatabase;

        [Tooltip("Fallback AudioData if the tag isn't found.")]
        [SerializeField] private AudioData defaultSurface;

        public void DSAPA()
        {
            Debug.Log("1. Trigger pulled! Shooting laser...");
            if (Physics.Raycast(checkOrigin.position, Vector3.down, out RaycastHit hit, raycastDistance, groundLayer))
            {
                Debug.Log("2. Laser HIT! Tag: " + hit.collider.tag);

                AudioData dataToP = GetAudioFT(hit.collider.tag);

                // 3. THE MISSING LINK: Send the data to the speaker!
                if (dataToP != null)
                {
                    Debug.Log("3. Playing sound!");
                    audioPlayer.PlaySWR(dataToP);
                }
            }
        }

        private AudioData GetAudioFT(string tag)
        {
            foreach (var s in surfaceDatabase)
            {
                if (s != null && s.nTag.Equals(tag, System.StringComparison.OrdinalIgnoreCase))
                {
                    return s;
                }
            }
            return defaultSurface;
        }
        private void OnDrawGizmosSelected()
        {
            if (checkOrigin != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(checkOrigin.position, checkOrigin.position + (Vector3.down * raycastDistance));
            }
        }
    }
}