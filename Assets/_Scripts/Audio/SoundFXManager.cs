using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField]
    private AudioSource soundFXObject;
    
    [SerializeField]
    private AudioMixerGroup mixerGroup;  // Reference to the Audio Mixer Group

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Play a specific sound at a given position with specified volume
    public void PlaySound(AudioClip clip, Transform transform, float volume)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        // Set the clip and volume
        audioSource.clip = clip;
        audioSource.volume = volume;
        // Assign the mixer group
        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;
        // Play the sound
        audioSource.Play();
        // Destroy the audio source after the clip has finished playing
        Destroy(audioSource.gameObject, clip.length);
    }

    // Play a random sound from an array at a given position with specified volume
    public void PlayRandomSound(AudioClip[] clips, Transform transform, float volume)
    {
        //assign random index
        int randomIndex = Random.Range(0, clips.Length);
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        // Set the clip and volume
        audioSource.clip = clips[randomIndex];
        audioSource.volume = volume;
        // Assign the mixer group
        if (mixerGroup != null)
            audioSource.outputAudioMixerGroup = mixerGroup;
        // Play the sound
        audioSource.Play();
        // Destroy the audio source after the clip has finished playing
        Destroy(audioSource.gameObject, clips[randomIndex].length);
    }
}
