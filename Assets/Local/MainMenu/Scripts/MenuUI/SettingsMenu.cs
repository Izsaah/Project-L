using UnityEngine;
using UnityEngine.UI;

namespace ProjectL.Global.Scripts.UI
{
    public class SettingsMenu : MonoBehaviour
    {
        [Tooltip("Drag the UI Panel that holds your settings here")]
        public GameObject settingsPanel;
        
        [Tooltip("Drag your Volume Slider here")]
        public Slider volumeSlider;

        [Tooltip("Drag any buttons you want to completely disappear (like Play, Quit) into this list")]
        public GameObject[] objectsToHide;

        void Start()
        {
            // Load the saved volume from the player's computer (default is 1, which is 100%)
            float savedVolume = PlayerPrefs.GetFloat("GlobalVolume", 1f);
            
            // Set the slider's physical position to match the saved volume
            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume;
            }
            
            // Apply the volume to the game
            ApplyVolume(savedVolume);

            // Make sure the panel is hidden when the main menu first loads
            CloseSettings();
        }

        // Call this from the "On Value Changed" event on your Slider
        public void SetVolume(float volume)
        {
            ApplyVolume(volume);
            
            // Save the setting so it remembers it next time they open the game
            PlayerPrefs.SetFloat("GlobalVolume", volume);
            PlayerPrefs.Save();
        }

        private void ApplyVolume(float volume)
        {
            // AudioListener is a built-in Unity feature that controls the master volume of everything!
            AudioListener.volume = volume;
        }

        // Call this from your "Cài đặt" Button's OnClick event
        public void OpenSettings()
        {
            if (settingsPanel != null) settingsPanel.SetActive(true);
            
            if (objectsToHide != null)
            {
                foreach (GameObject obj in objectsToHide)
                {
                    if (obj != null) obj.SetActive(false);
                }
            }
        }

        // Call this from your "Đóng" (Close/X) Button's OnClick event
        public void CloseSettings()
        {
            if (settingsPanel != null) settingsPanel.SetActive(false);
            
            if (objectsToHide != null)
            {
                foreach (GameObject obj in objectsToHide)
                {
                    if (obj != null) obj.SetActive(true);
                }
            }
        }
    }
}
