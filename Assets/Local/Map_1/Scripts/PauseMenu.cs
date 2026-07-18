using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectL.Local.Map_1
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool GameIsPaused = false;

        public GameObject pauseMenuUI;
        public string mainMenuSceneName = "MainMenu";
        // ADDED: This runs exactly once the moment the level loads.
        // It forces the game to unpause and hides the menu, just in case!
        void Start()
        {
            GameIsPaused = false;
            Time.timeScale = 1f;
            pauseMenuUI.SetActive(false);
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
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
    }

}
