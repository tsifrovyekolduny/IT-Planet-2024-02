using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class NewBehaviourScript : Singletone<NewBehaviourScript>
{
    [SerializeField] private AudioMixer audioMixer;    

    public void SetSoundsVolume(float level)
    {
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
}
