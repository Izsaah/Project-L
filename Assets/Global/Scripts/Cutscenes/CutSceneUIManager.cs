using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace ProjectL.Global.Script.CutScenes
{

    public class CutSceneUIManager : MonoBehaviour
    {
        [Header("GET MAH DBBBBBBBBBBBBBBBB IN HERRRRRRRRRRE YOU STUPIF DCK")]
        public CutsceneDatabase db;
        [Header("WOOOOOOOO WHAT DO YOU SEE YOUR MOM HAHAHAAH")]
        [Tooltip("the ONE single Image componet in your ASSHAHAAHAHAHAHA")]
        public Image dbImage;
        [Tooltip("THE BUUUUUUUUUUTOOOOOOOOON")]
        public GameObject bgBlocker;

        [Header("ReplayEvents")]
        [Tooltip("Put what to do when finish")]
        public UnityEvent Finish;

        private Sprite[] cSequence;
        private int cSI = 0;
        private void Awake()
        {
            hideAllImages();
        }

        public void ShowImage(string id)
        {
            if (db == null || dbImage == null) return;

            cSequence = db.getImage(id);

            if (cSequence == null || cSequence.Length == 0)
            {
                Debug.LogWarning($"CutSceneUIManager: image key not found: {id}");
                return;
            }
            cSI = 0;
            dbImage.sprite = cSequence[cSI];
            dbImage.gameObject.SetActive(true);

            if (bgBlocker != null)
            {
                bgBlocker.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void NextSlide()
        {
            cSI++;
            if (cSI < cSequence.Length)
            {
                dbImage.sprite = cSequence[cSI];
            }
            else
            {
                hideAllImages();
            }
        }
        public void hideAllImages()
        {
            if (dbImage != null)
            {
                dbImage.gameObject.SetActive(false);
            }

            if (bgBlocker != null)
            {
                bgBlocker.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Finish?.Invoke();
        }
    }
}

