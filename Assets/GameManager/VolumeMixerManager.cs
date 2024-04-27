using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetSoundsVolume(float level)
    {
        audioMixer.SetFloat("SoundsVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", level);
    }
}
