using System.Collections;
using UnityEngine;
namespace ProjectL.Local.Map_1
{
    public class LightningGenerator : MonoBehaviour
    {
        [Header("Visuals")]
        public Light lightningLight;
        
        [Header("Audio")]
        public AudioSource thunderSound;

        [Header("Timing")]
        public float minTime = 5f;
        public float maxTime = 15f;

        private void Start()
        {
            if (lightningLight == null) lightningLight = GetComponent<Light>();
            lightningLight.enabled = false;
            StartCoroutine(LightningLoop());
        }

        private IEnumerator LightningLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTime, maxTime));

                // Play the thunder sound!
                if (thunderSound != null)
                {
                    thunderSound.Play();
                }

                // Flash
                lightningLight.enabled = true;
                yield return new WaitForSeconds(0.1f);
                lightningLight.enabled = false;

                // Double flash chance
                if (Random.value > 0.5f)
                {
                    yield return new WaitForSeconds(0.05f);
                    lightningLight.enabled = true;
                    yield return new WaitForSeconds(0.1f);
                    lightningLight.enabled = false;
                }
            }
        }
    }


}
