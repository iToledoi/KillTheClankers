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
        // Convert linear 0-1 volume to logarithmic scale for AudioMixer
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }
    
}
