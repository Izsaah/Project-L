using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SceneName = "Map_1";
    public void PlayGame()
    {
        // Default play game (will act as continue if save exists, otherwise new game)
        ContinueGame();
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
            NewGame();
        }
    }

    public void NewGame()
    {
        // Delete save file if starting a completely new game
        string saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, "savegame.json");
        if (System.IO.File.Exists(saveFilePath))
        {
            System.IO.File.Delete(saveFilePath);
        }

        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
