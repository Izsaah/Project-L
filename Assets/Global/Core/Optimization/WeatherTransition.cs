using UnityEngine;

public class WeatherTransition : MonoBehaviour
{
    [Header("Daytime Stuff (Will turn OFF)")]
    public GameObject sunLight;
    public GameObject dayGlobalVolume;
    public AudioSource birdSounds;

    [Header("Stormy Night Stuff (Will turn ON)")]
    public GameObject nightGlobalVolume;
    public GameObject lightningSystem;
    public AudioSource rainSounds;
    public ParticleSystem rainParticles;

    // We will call this method from the Inspector!
    public void MakeItStormy()
    {
        // 1. Kill the sunny day
        if (sunLight != null) sunLight.SetActive(false);
        if (dayGlobalVolume != null) dayGlobalVolume.SetActive(false);
        if (birdSounds != null) birdSounds.Stop();

        // 2. Start the terrifying storm
        if (nightGlobalVolume != null) nightGlobalVolume.SetActive(true);
        if (lightningSystem != null) lightningSystem.SetActive(true);
        if (rainSounds != null) rainSounds.Play();
        if (rainParticles != null) rainParticles.Play();
    }
}
