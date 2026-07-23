using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ProjectL.Global.Core.GameMaster;

namespace ProjectL.Global.Script.CutScenes
{
    public class CutSceneUIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        public Image dbImage;
        public GameObject bgBlocker;

        [Header("Animation Settings")]
        public bool useFade = true;
        public float fadeSpeed = 2f;

        [Header("ReplayEvents")]
        public UnityEvent Finish;

        public static CutSceneUIManager Instance;
        private Sprite[] cSequence;
        private ProjectL.Global.Script.Audio.AudioData[] cSounds;
        private int cSI = 0;

        private bool isTransitioning = false;

        private void Awake()
        {
            Instance = this;
            // FIXED: Just silently hide the UI on startup. Don't touch the mouse or events!
            if (dbImage != null) dbImage.gameObject.SetActive(false);
            if (bgBlocker != null) bgBlocker.SetActive(false);
        }

        public void ShowImage(CutsceneDatabase data)
        {
            if (data == null || data.images == null || data.images.Length == 0) return;
            if (useFade && isTransitioning) return;

            cSequence = data.images;
            cSounds = data.sounds;
            cSI = 0;

            dbImage.sprite = cSequence[cSI];

            dbImage.gameObject.SetActive(true);
            if (bgBlocker != null) bgBlocker.SetActive(true);

            // Automatically hide objective text during cutscenes
            if (ProjectL.Global.Scripts.Objectives.ObjectiveManager.Instance != null)
            {
                ProjectL.Global.Scripts.Objectives.ObjectiveManager.Instance.HideObjective();
            }

            // Unlock mouse for the cutscene
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (fadeSpeed <= 0f) fadeSpeed = 2f;

            if (useFade)
            {
                Color c = dbImage.color;
                c.a = 0f;
                dbImage.color = c;
                StartCoroutine(FadeInSlide());
            }
            else
            {
                Color c = dbImage.color;
                c.a = 1f;
                dbImage.color = c;
                PlaySlideSound();
            }
        }

        public void NextSlide()
        {
            if (useFade)
            {
                if (isTransitioning) return;
                StartCoroutine(FadeToNextSlide());
            }
            else
            {
                cSI++;
                if (cSI < cSequence.Length)
                {
                    dbImage.sprite = cSequence[cSI];
                    PlaySlideSound();
                }
                else
                {
                    hideAllImages();
                }
            }
        }

        private IEnumerator FadeInSlide()
        {
            isTransitioning = true;
            PlaySlideSound();

            Color c = dbImage.color;
            while (c.a < 1f)
            {
                c.a += Time.unscaledDeltaTime * fadeSpeed;
                dbImage.color = c;
                yield return null;
            }

            c.a = 1f;
            dbImage.color = c;
            isTransitioning = false;
        }

        private IEnumerator FadeToNextSlide()
        {
            isTransitioning = true;

            Color c = dbImage.color;
            while (c.a > 0f)
            {
                c.a -= Time.unscaledDeltaTime * fadeSpeed;
                dbImage.color = c;
                yield return null;
            }
            c.a = 0f;
            dbImage.color = c;

            cSI++;
            if (cSI < cSequence.Length)
            {
                dbImage.sprite = cSequence[cSI];

                PlaySlideSound();
                while (c.a < 1f)
                {
                    c.a += Time.unscaledDeltaTime * fadeSpeed;
                    dbImage.color = c;
                    yield return null;
                }
                c.a = 1f;
                dbImage.color = c;
                isTransitioning = false;
            }
            else
            {
                isTransitioning = false;
                hideAllImages();
            }
        }

        private void PlaySlideSound()
        {
            if (cSounds != null && cSI < cSounds.Length && cSounds[cSI] != null)
            {
                if (GameManager.Instance != null && GameManager.Instance.audioManager != null)
                {
                    GameManager.Instance.audioManager.pSS(cSounds[cSI]);
                }
            }
        }

        public void hideAllImages()
        {
            if (dbImage != null) dbImage.gameObject.SetActive(false);
            if (bgBlocker != null) bgBlocker.SetActive(false);

            // Show objective text again after cutscene ends
            if (ProjectL.Global.Scripts.Objectives.ObjectiveManager.Instance != null)
            {
                ProjectL.Global.Scripts.Objectives.ObjectiveManager.Instance.ShowObjective();
            }

            // Lock the mouse back to the center of the screen when cutscene ends!
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Finish?.Invoke();
        }
    }
}
