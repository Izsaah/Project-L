using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectL.Global.Scripts
{
    public class BootManager : MonoBehaviour
    {
        [Tooltip("The name of the scene to load after initialization (e.g. MainMenu)")]
        public string nextSceneName = "MainMenu";

        private IEnumerator Start()
        {
            // Optional: You can put a loading screen here

            // Wait 1 frame to ensure all Awake() and Start() methods on GameMaster singletons have finished
            yield return null;

            // Load the Main Menu!
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
