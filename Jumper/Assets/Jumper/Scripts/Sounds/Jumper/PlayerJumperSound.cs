using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayerJumperSound : MonoBehaviour
{
    [Header("Клипы звуков приземления")]
    public AudioClip[] AudioClipsBlows;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Играть звук удара
    public void PlaySoundBlow()
    {
        _audioSource.clip = AudioClipsBlows[Random.Range(0, AudioClipsBlows.Length)];
        _audioSource.Play();
    }
}
