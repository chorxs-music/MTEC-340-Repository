using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class BulletAudio : MonoBehaviour
{
    public static BulletAudio Instance;
    public GameState State;
    AudioSource _source;

    [SerializeField] AudioClip _bulletCollision;
    [SerializeField] AudioClip _bulletDestroy;
    public AudioClip _playerDeath;

    void Awake()
    {
        if (Instance != this && Instance != null)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.enabled = true;
    }

    public void shipDestroy()
    {
        _source.PlayOneShot(_bulletDestroy);
    }

    public void bulletDestroy()
    {
        _source.PlayOneShot(_bulletCollision);
    }

    public void playerDeath()
    {
        _source.PlayOneShot(_playerDeath);
    }
}
