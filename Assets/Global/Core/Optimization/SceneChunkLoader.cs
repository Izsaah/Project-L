using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ProjectL.Global.Core.Optimization
{

    public class ChunkLoader : MonoBehaviour
    {
        [Header("Which Map Chunk is this ?")]
        [Tooltip("type the EXACT name of the  scene file here")]
        public string sceneToLoad;

        private bool isLoaded = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isLoaded)
            {
                StartCoroutine(LoadChunk());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && isLoaded)
            {
                StartCoroutine(UnloadChunk());
            }
        }
        private IEnumerator LoadChunk()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            isLoaded = true;
            Debug.Log($"Success loading:{sceneToLoad}");
        }

        private IEnumerator UnloadChunk()
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToLoad);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            isLoaded = false;
            Debug.Log($"Success Unloading:{sceneToLoad}");
        }
    }


}
