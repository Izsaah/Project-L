using UnityEngine;
using UnityEngine.SceneManagement;
using ProjectL.Global.Script.CutScenes;

namespace ProjectL.Global.Core.GameMaster
{
    public class MainEndManager : MonoBehaviour
    {
        [Header("End Game Settings")]
        [Tooltip("How many NPCs/Interactions need to be completed to win?")]
        public int targetNPCCount = 4;

        [Header("Cutscene Files")]
        public CutsceneDatabase timeOutCutscene;
        public CutsceneDatabase successCutscene;


        // Safety flag so we don't trigger a win and a lose at the exact same time
        private bool isGameEnded = false;

        private void Start()
        {
            // Subscribe to the deadline event from your TimeManager
            if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
            {
                GameManager.Instance.timeManager.OnDeadlineReached += HandleTimeOut;
            }
            else
            {
                Debug.LogWarning("MainEndManager: GameManager or TimeManager is missing in the scene!");
            }
        }

        // ==========================================
        // CONDITION 1: TIME RUNS OUT
        // ==========================================
        private void HandleTimeOut()
        {
            // If the game already ended (player won), do nothing
            if (isGameEnded) return;

            isGameEnded = true;
            Debug.Log("Deadline Reached! Triggering Time Out Cutscene.");

            if (CutSceneUIManager.Instance != null)
            {
                // Play the timeout sequence
                CutSceneUIManager.Instance.ShowImage(timeOutCutscene);

                // Tell the Cutscene Manager to load the Main Menu ONLY when this specific cutscene finishes
                CutSceneUIManager.Instance.Finish.AddListener(ReturnToMenu);
            }
        }

        // ==========================================
        // CONDITION 2: ALL NPCS HELPED
        // ==========================================
        public void CheckNPCCount()
        {
            // If the game already ended (time ran out), do nothing
            if (isGameEnded) return;

            // Check how many interactions have been saved to the WorldStateManager
            int currentCompleted = GameManager.Instance.worldState.destroyedObjects.Count;

            if (currentCompleted >= targetNPCCount)
            {
                isGameEnded = true;
                Debug.Log("All NPCs helped! Triggering Success Cutscene.");

                if (CutSceneUIManager.Instance != null)
                {
                    // Play the success sequence
                    CutSceneUIManager.Instance.ShowImage(successCutscene);

                    // Tell the Cutscene Manager to load the Main Menu ONLY when this specific cutscene finishes
                    CutSceneUIManager.Instance.Finish.AddListener(ReturnToMenu);
                }
            }
            else
            {
                Debug.Log($"NPC helped! Current count: {currentCompleted} / {targetNPCCount}");
            }
        }

        // ==========================================
        // SCENE LOADING (Triggered by Cutscene End)
        // ==========================================
        private void ReturnToMenu()
        {
            // Make sure to remove the listener so it doesn't cause memory leaks
            CutSceneUIManager.Instance.Finish.RemoveListener(ReturnToMenu);

            // NOTE: Make sure "MainMenu" is the exact spelling of your main menu scene!
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            // Always unsubscribe from events when this object is destroyed to prevent memory leaks
            if (GameManager.Instance != null && GameManager.Instance.timeManager != null)
            {
                GameManager.Instance.timeManager.OnDeadlineReached -= HandleTimeOut;
            }
        }
    }
}
