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
            
            // Delete save file if starting a completely new game via direct LoadLevel
            string saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
            if (System.IO.File.Exists(saveFilePath))
            {
                System.IO.File.Delete(saveFilePath);
            }
            
            SceneManager.LoadScene(k);
        }

        public void ContinueGame()
        {
            if (ProjectL.Global.Script.SaveSystem.SaveLoadManager.Instance != null && 
                System.IO.File.Exists(System.IO.Path.Combine(Application.persistentDataPath, "savegame.json")))
            {
                ProjectL.Global.Script.SaveSystem.SaveLoadManager.Instance.LoadGame();
            }
            else
            {
                Debug.LogWarning("No save file found! Cannot continue.");
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
    }

}
