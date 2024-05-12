using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager s_Instance;
    private AudioMixer _mixer;

    [SerializeField] private AudioSource soundObject;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        InitializeManager();
    }

    private void InitializeManager()
    {
        // ������ ���-�� �� �������� ������ (����� ���� �����������);
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

        int rand = Random.Range(0, audioClip.Length - 1);

        audioSource.clip = audioClip[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float audioLength = audioSource.clip.length;

        Destroy(audioSource.gameObject, audioLength);
    }
}
