using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSoundsVolume(float level)
    {
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
}
