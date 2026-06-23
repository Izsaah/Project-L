using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        private void Awake()
        {
            hideAllImages();
        }

        public void ShowImage(string id)
        {
            if (db == null)
            {
                Debug.LogWarning("CutSceneUIManager: db is missing.");
                return;
            }

            if (dbImage == null)
            {
                Debug.LogWarning("CutSceneUIManager: dbImage is missing.");
                return;
            }

            Sprite sprite = db.getImage(id);

            if (sprite == null)
            {
                Debug.LogWarning($"CutSceneUIManager: image key not found: {id}");
                return;
            }

            dbImage.sprite = sprite;
            dbImage.gameObject.SetActive(true);

            if (bgBlocker != null)
            {
                bgBlocker.SetActive(true);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
        }
    }
}

