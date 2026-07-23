using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectL.Global.Script.SaveSystem
{
    public static class AutoSettingsLoader
    {
        // This attribute tells Unity to run this method automatically as soon as the game boots up!
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeSettings()
        {
            // Apply the global volume immediately upon boot
            float savedVolume = PlayerPrefs.GetFloat("GlobalVolume", 1f);
            AudioListener.volume = savedVolume;

            // Also, listen for every time ANY new scene finishes loading
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Just to be absolutely safe, re-apply the global volume on scene load
            float savedVolume = PlayerPrefs.GetFloat("GlobalVolume", 1f);
            AudioListener.volume = savedVolume;
        }
    }
}
