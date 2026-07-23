using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectL.Local.Map_1
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;

        private ProjectL.Scripts.Interface.IInputProvider inputProvider;

        public GameObject pauseMenuUI;
        public string mainMenuSceneName = "MainMenu";
        // ADDED: This runs exactly once the moment the level loads.
        // It forces the game to unpause and hides the menu, just in case!
        void Start()
        {
            GameIsPaused = false;
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);

            inputProvider = FindAnyObjectByType<ProjectL.Global.Script.Player.PlayerInputHandler>();
        }
        void Update()
        {
            if (inputProvider != null && inputProvider.GetPause() || Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        public void LoadMenu()
        {
            Time.timeScale = 1f;
            GameIsPaused = false;

            SceneManager.LoadScene(mainMenuSceneName);
        }

        public void SaveGame()
        {
            if (ProjectL.Global.Script.SaveSystem.SaveLoadManager.Instance != null)
            {
                ProjectL.Global.Script.SaveSystem.SaveLoadManager.Instance.SaveGame();
            }
            else
            {
                Debug.LogWarning("SaveLoadManager is missing! Cannot save game.");
            }
        }

        public void LoadGame()
        {
            if (ProjectL.Global.Script.SaveSystem.SaveLoadManager.Instance != null)
            {
                // Unpause the game before loading to ensure time flows normally after load
                Time.timeScale = 1f;
                GameIsPaused = false;
                ProjectL.Global.Script.SaveSystem.SaveLoadManager.Instance.LoadGame();
            }
            else
            {
                Debug.LogWarning("SaveLoadManager is missing! Cannot load game.");
            }
        }
    }

}
