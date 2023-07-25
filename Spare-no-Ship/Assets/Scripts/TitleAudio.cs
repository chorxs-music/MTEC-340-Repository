using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleAudio : MonoBehaviour
{
    private static bool isAudioPlaying = false;

    [SerializeField] AudioClip _mainSoundtrack;
    [SerializeField] AudioSource _audioSource;
    AudioSource _source;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        PlaySoundtrack();
    }

    private void PlaySoundtrack()
    {
        if (!isAudioPlaying)
        {
            _source.clip = _mainSoundtrack;
            _source.Play();
            isAudioPlaying = true;
        }
    }
}
