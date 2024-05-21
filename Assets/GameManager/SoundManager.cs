using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singletone<SoundManager>
{    
    //private AudioMixer _mixer;

    [SerializeField] private AudioSource soundObject;    

    private void Awake()
    {
        InitializeManager();
    }

    private void InitializeManager()
    {
        // делать что-то на загрузке уровня (когда игра запустилась);
    }

    public void PlayAudioClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, audioLength);
    }

    public void PlayAudioClip(AudioClip[] audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);

        int rand = Random.Range(0, audioClip.Length);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, audioLength);
    }
}
