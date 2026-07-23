using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectL.Global.Script.Player;

namespace ProjectL.Global.Scripts.UI
{
    public class KeybindUI : MonoBehaviour
    {
        [Tooltip("The action you want this button to rebind")]
        public InputAction actionToRebind;
        
        [Tooltip("The text component on your button (TextMeshPro)")]
        public TextMeshProUGUI buttonText;

        private bool isWaitingForKey = false;
        private string prefKey;

        void Start()
        {
            prefKey = "KeyBind_" + actionToRebind.ToString();

            // Display current saved key or a default if none exists yet
            if (PlayerPrefs.HasKey(prefKey))
            {
                buttonText.text = PlayerPrefs.GetString(prefKey);
            }
            else
            {
                buttonText.text = "NOT SET";
            }
        }

        // Hook this method up to your UI Button's OnClick event!
        public void StartRebinding()
        {
            if (isWaitingForKey) return;
            
            isWaitingForKey = true;
            buttonText.text = "PRESS ANY KEY...";

            // VERY IMPORTANT: Deselect the button so pressing Space doesn't just click it again!
            if (UnityEngine.EventSystems.EventSystem.current != null)
            {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }
        }

        void OnGUI()
        {
            if (isWaitingForKey)
            {
                Event e = Event.current;
                if (e.isKey && e.type == EventType.KeyDown)
                {
                    // Ignore empty keycodes (sometimes modifier keys do this)
                    if (e.keyCode == KeyCode.None) return;

                    // Backspace cancels the rebind!
                    if (e.keyCode == KeyCode.Backspace)
                    {
                        buttonText.text = PlayerPrefs.GetString(prefKey, "NOT SET");
                        isWaitingForKey = false;
                        return;
                    }

                    // Save the new key
                    PlayerPrefs.SetString(prefKey, e.keyCode.ToString());
                    PlayerPrefs.Save();

                    // Update UI text
                    buttonText.text = e.keyCode.ToString();

                    // If the player is currently in-game, find the input handler and update it instantly!
                    var handler = FindAnyObjectByType<PlayerInputHandler>();
                    if (handler != null)
                    {
                        handler.ReloadKeyBindings();
                    }

                    isWaitingForKey = false;
                }
            }
        }
    }
}
