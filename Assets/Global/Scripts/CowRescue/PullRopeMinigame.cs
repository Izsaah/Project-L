using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace ProjectL.Global.Script.CowRescue
{
    /// <summary>
    /// QTE Timing Bar Minigame:
    /// - A horizontal bar appears at the bottom of the screen
    /// - A red fill sweeps from left to right
    /// - An "E" target zone sits at a random position (width varies: easy/hard)
    /// - Player presses E when the red fill overlaps the E zone
    /// - Difficulty alternates: hard → easy → hard → easy
    /// - 3-second pause between rounds
    /// - 7-8 second countdown before the first round
    /// </summary>
    public class PullRopeMinigame : MonoBehaviour
    {
        [Header("UI References (auto-built)")]
        [SerializeField] private GameObject minigamePanel;     // The full-screen overlay
        [SerializeField] private RectTransform barBackground;  // The dark bar frame
        [SerializeField] private RectTransform sweepFill;      // The red moving fill
        [SerializeField] private RectTransform targetZoneUI;   // The "E" target zone
        [SerializeField] private TextMeshProUGUI targetZoneText; // "E" letter inside target
        [SerializeField] private TextMeshProUGUI statusText;   // "Round 3/10 | Hit: 5"
        [SerializeField] private TextMeshProUGUI countdownText;// Big countdown text "3... 2... 1..."
        [SerializeField] private TextMeshProUGUI resultFlash;  // Flash "OK!" or "MISS!"

        [Header("3D Scene References")]
        [SerializeField] public Transform cowTransform;
        [SerializeField] public Transform manTransform;
        [SerializeField] private LineRenderer ropeRenderer;
        [SerializeField] private Transform playerRopeAnchor;
        [SerializeField] private Transform cowRopeAnchor;

        [Header("Cow Movement")]
        [SerializeField] private Vector3 cowExitDirection = Vector3.forward;
        [SerializeField] private float cowExitDistance = 3f;

        [Header("Timing Settings")]
        [SerializeField] private float sweepDuration = 1.2f;       // How long the red bar takes to sweep fully
        [SerializeField] private float preparationTime = 8f;       // Countdown before first round
        [SerializeField] private float delayBetweenRounds = 3f;    // Pause between rounds

        [Header("Difficulty Settings")]
        [SerializeField] private float easyZoneWidth = 0.2f;       // 20% of bar width (easy)
        [SerializeField] private float hardZoneWidth = 0.08f;      // 8% of bar width (hard)
        [SerializeField] private int totalRounds = 10;
        [SerializeField] private int requiredSuccesses = 7;
        [SerializeField] private int missesForHelper = 2;

        [Header("Events")]
        public UnityEvent onNPCHelperNeeded;
        public UnityEvent onSuccessHit;
        public UnityEvent onMissHit;

        // --- State ---
        private int currentRound = 0;
        private int successCount = 0;
        private int consecutiveMisses = 0;
        private bool npcHelperActivated = false;
        private bool isActive = false;
        private bool isSweeping = false;
        private bool playerPressedThisRound = false;
        private float sweepProgress = 0f;  // 0 to 1
        private float currentTargetMin, currentTargetMax;
        private Action<bool> onCompleteCallback;
        private Vector3 cowStartPos;

        private void Awake()
        {
            if (minigamePanel != null) minigamePanel.SetActive(false);
            if (ropeRenderer != null) ropeRenderer.enabled = false;
        }

        // ==================== PUBLIC API ====================

        public void StartMinigame(Action<bool> onComplete)
        {
            onCompleteCallback = onComplete;
            currentRound = 0;
            successCount = 0;
            consecutiveMisses = 0;
            npcHelperActivated = false;
            isActive = true;

            if (cowTransform != null) cowStartPos = cowTransform.position;
            if (ropeRenderer != null) ropeRenderer.enabled = true;

            // Show panel, start countdown
            if (minigamePanel != null) minigamePanel.SetActive(true);
            HideSweepUI();

            StartCoroutine(CountdownThenStart());
        }

        // ==================== COUNTDOWN ====================

        private IEnumerator CountdownThenStart()
        {
            // Show countdown text
            if (countdownText != null) countdownText.gameObject.SetActive(true);
            if (statusText != null) statusText.text = "Chuan bi...";
            if (resultFlash != null) resultFlash.gameObject.SetActive(false);

            float remaining = preparationTime;
            while (remaining > 0f)
            {
                if (countdownText != null)
                    countdownText.text = Mathf.CeilToInt(remaining).ToString();
                remaining -= Time.deltaTime;
                yield return null;
            }

            if (countdownText != null) countdownText.gameObject.SetActive(false);

            // Start first round
            StartNextRound();
        }

        // ==================== ROUND LOGIC ====================

        private void StartNextRound()
        {
            if (!isActive) return;

            currentRound++;
            if (currentRound > totalRounds)
            {
                EndMinigame();
                return;
            }

            playerPressedThisRound = false;
            sweepProgress = 0f;
            isSweeping = true;

            // Determine difficulty: alternate hard/easy (round 1=hard, 2=easy, 3=hard...)
            bool isHardRound = (currentRound % 2 == 1);
            float zoneWidth = isHardRound ? hardZoneWidth : easyZoneWidth;

            // Random position for the target zone (keep it away from edges)
            float center = UnityEngine.Random.Range(0.3f, 0.85f);
            currentTargetMin = Mathf.Clamp(center - zoneWidth / 2f, 0.05f, 0.95f);
            currentTargetMax = Mathf.Clamp(center + zoneWidth / 2f, 0.05f, 0.95f);

            // Update UI
            ShowSweepUI();
            UpdateTargetZonePosition();
            UpdateStatusText();

            // Reset target zone color to gray
            if (targetZoneUI != null)
            {
                Image tzImg = targetZoneUI.GetComponent<Image>();
                if (tzImg != null) tzImg.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            }

            // Reset sweep fill
            if (sweepFill != null)
            {
                sweepFill.anchorMin = new Vector2(0f, 0f);
                sweepFill.anchorMax = new Vector2(0f, 1f);
                sweepFill.offsetMin = Vector2.zero;
                sweepFill.offsetMax = Vector2.zero;
            }
        }

        private void Update()
        {
            if (!isActive || !isSweeping) return;

            // Advance the sweep
            sweepProgress += Time.deltaTime / sweepDuration;
            sweepProgress = Mathf.Clamp01(sweepProgress);

            // Update red fill: grows from left (anchor 0,0 to sweepProgress,1)
            if (sweepFill != null)
            {
                sweepFill.anchorMin = new Vector2(0f, 0f);
                sweepFill.anchorMax = new Vector2(sweepProgress, 1f);
                sweepFill.offsetMin = Vector2.zero;
                sweepFill.offsetMax = Vector2.zero;
            }

            // Update rope is now in LateUpdate

            // Check E or Space press
            if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) && !playerPressedThisRound)
            {
                playerPressedThisRound = true;
                isSweeping = false;
                EvaluatePress();
            }

            // If sweep reaches end without pressing
            if (sweepProgress >= 1f && !playerPressedThisRound)
            {
                playerPressedThisRound = true;
                isSweeping = false;
                // Auto-miss
                RegisterMiss();
            }
        }

        private void LateUpdate()
        {
            if (isActive && ropeRenderer != null && ropeRenderer.enabled)
            {
                UpdateRope();
            }
        }

        private void EvaluatePress()
        {
            // Check if sweepProgress is within the target zone
            if (sweepProgress >= currentTargetMin && sweepProgress <= currentTargetMax)
            {
                RegisterHit();
            }
            else
            {
                RegisterMiss();
            }
        }

        private void RegisterHit()
        {
            successCount++;
            consecutiveMisses = 0;
            onSuccessHit?.Invoke();

            // Move cow smoothly
            if (cowTransform != null)
            {
                float progress = (float)successCount / requiredSuccesses;
                Vector3 targetPos = cowStartPos + cowExitDirection.normalized * (progress * cowExitDistance);
                StartCoroutine(SmoothMoveCow(targetPos, 0.8f)); // Move over 0.8 seconds
            }

            // Change target zone color to green
            if (targetZoneUI != null)
            {
                Image tzImg = targetZoneUI.GetComponent<Image>();
                if (tzImg != null) tzImg.color = Color.green;
            }

            // Flash OK
            StartCoroutine(FlashResult(true));
        }

        private void RegisterMiss()
        {
            consecutiveMisses++;
            onMissHit?.Invoke();

            // NPC helper
            if (consecutiveMisses >= missesForHelper && !npcHelperActivated)
            {
                npcHelperActivated = true;
                onNPCHelperNeeded?.Invoke();
                if (manTransform != null) manTransform.gameObject.SetActive(true);
            }

            StartCoroutine(FlashResult(false));
        }

        private IEnumerator FlashResult(bool isHit)
        {
            UpdateStatusText();
            
            // Keep the sweep UI visible for 1 second so the player can see the hit/miss result
            yield return new WaitForSeconds(1f);
            
            HideSweepUI();

            // Wait remaining delay between rounds
            yield return new WaitForSeconds(delayBetweenRounds - 1f);

            // Check if game should end
            if (currentRound >= totalRounds)
            {
                EndMinigame();
            }
            else
            {
                StartNextRound();
            }
        }

        // ==================== UI & ANIMATION HELPERS ====================

        private System.Collections.IEnumerator SmoothMoveCow(Vector3 targetPos, float duration)
        {
            if (cowTransform == null) yield break;

            Animator cowAnim = cowTransform.GetComponent<Animator>();
            if (cowAnim != null) cowAnim.SetBool("isWalking", true);

            Vector3 startPos = cowTransform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                cowTransform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                yield return null;
            }
            cowTransform.position = targetPos;

            if (cowAnim != null) cowAnim.SetBool("isWalking", false);
        }

        private void UpdateTargetZonePosition()
        {
            if (targetZoneUI != null)
            {
                targetZoneUI.anchorMin = new Vector2(currentTargetMin, 0f);
                targetZoneUI.anchorMax = new Vector2(currentTargetMax, 1f);
                targetZoneUI.offsetMin = Vector2.zero;
                targetZoneUI.offsetMax = Vector2.zero;
            }
        }

        private void UpdateStatusText()
        {
            if (statusText != null)
            {
                bool isHard = (currentRound % 2 == 1);
                string diff = isHard ? "KHO" : "DE";
                statusText.text = $"Vong {currentRound}/{totalRounds}  |  Thanh cong: {successCount}/{requiredSuccesses}  |  {diff}";
            }
        }

        private void ShowSweepUI()
        {
            if (barBackground != null) barBackground.gameObject.SetActive(true);
            if (sweepFill != null) sweepFill.gameObject.SetActive(true);
            if (targetZoneUI != null) targetZoneUI.gameObject.SetActive(true);
        }

        private void HideSweepUI()
        {
            if (barBackground != null) barBackground.gameObject.SetActive(false);
            if (sweepFill != null) sweepFill.gameObject.SetActive(false);
            if (targetZoneUI != null) targetZoneUI.gameObject.SetActive(false);
        }

        private void UpdateRope()
        {
            if (ropeRenderer == null) return;
            if (playerRopeAnchor != null && cowRopeAnchor != null)
            {
                ropeRenderer.SetPosition(0, playerRopeAnchor.position);
                ropeRenderer.SetPosition(1, cowRopeAnchor.position);
            }
        }

        // ==================== END GAME ====================

        private void EndMinigame()
        {
            isActive = false;
            isSweeping = false;
            if (minigamePanel != null) minigamePanel.SetActive(false);
            if (ropeRenderer != null) ropeRenderer.enabled = false;

            bool win = successCount >= requiredSuccesses;
            onCompleteCallback?.Invoke(win);
        }
    }
}
