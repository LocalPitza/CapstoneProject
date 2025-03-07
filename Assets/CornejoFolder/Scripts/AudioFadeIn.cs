using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFadeIn : MonoBehaviour
{
    public AudioSource audioSource; // Assign your AudioSource in the Inspector
    public float fadeInDuration = 3.0f; // Time in seconds to reach full volume
    private float targetVolume;

    void Start()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            targetVolume = audioSource.volume;
            audioSource.volume = 0; // Start with volume at 0
            audioSource.Play(); // Start playing the music
            StartCoroutine(FadeInAudio());
        }
    }

    IEnumerator FadeInAudio()
    {
        float currentTime = 0;

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, targetVolume, currentTime / fadeInDuration);
            yield return null;
        }
        audioSource.volume = targetVolume; // Ensure volume is fully set at the end
    }
}
