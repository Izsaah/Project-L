using UnityEngine;
using ProjectL.Global.Core.GameMaster;
using ProjectL.Global.Script.CutScenes;
using ProjectL.Global.Script.Audio;

public class GlobalEventBridge : MonoBehaviour
{
    // The slot for Cutscenes!
    public void PlayCutscene(CutsceneDatabase cutsceneFile)
    {
        if (CutSceneUIManager.Instance != null)
        {
            CutSceneUIManager.Instance.ShowImage(cutsceneFile);
        }
    }

    // The slot for Audio!
    public void PlayGlobalSound(AudioData audioFile)
    {
        if (GameManager.Instance != null && GameManager.Instance.audioManager != null)
        {
            GameManager.Instance.audioManager.pSS(audioFile);
        }
    }
}
