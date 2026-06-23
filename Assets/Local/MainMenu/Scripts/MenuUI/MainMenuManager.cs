using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ProjectL.Global.Core.GameMaster
{
    public class MainMenuManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void LoadLevel(string k)
        {
            Debug.Log($"Loading level: {k}");
            SceneManager.LoadScene(k);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }

}
