using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField]
    private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }
    public void PlaySound(AudioClip clip, Transform transform, float volume)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        // Set the clip and volume
        audioSource.clip = clip;
        audioSource.volume = volume;
        // Play the sound
        audioSource.Play();
        // Destroy the audio source after the clip has finished playing
        Destroy(audioSource.gameObject, clip.length);
    }

    public void PlayRandomSound(AudioClip[] clips, Transform transform, float volume)
    {
        //assign random index
        int randomIndex = Random.Range(0, clips.Length);
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, transform.position, Quaternion.identity);
        // Set the clip and volume
        audioSource.clip = clips[randomIndex];
        audioSource.volume = volume;
        // Play the sound
        audioSource.Play();
        // Destroy the audio source after the clip has finished playing
        Destroy(audioSource.gameObject, clips[randomIndex].length);
    }
}
