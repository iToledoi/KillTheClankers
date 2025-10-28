using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{

    [SerializeField]
    private AudioMixer audioMixer;

    // Set the master volume in game using AudioMixer
    public void SetMasterVolume(float level)
    {   
        if (audioMixer == null)
        {
            Debug.LogError("Audio Mixer not assigned in SoundMixerManager!");
            return;
        }

        // Ensure level is between 0 and 1
        level = Mathf.Clamp01(level);
        
        // Convert slider value to decibels
        float dB;
        if (level <= 0)
            dB = -80f; // Approximately silence
        else
            dB = Mathf.Log10(level) * 20f;

        audioMixer.SetFloat("masterVolume", dB);
        Debug.Log($"Setting volume to {dB}dB (level: {level})");
    }
    
}
