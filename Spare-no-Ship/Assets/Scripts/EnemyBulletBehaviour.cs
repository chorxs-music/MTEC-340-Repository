using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public float _bulletSpeed = 10.0f;
    public GameState State;
    public Transform bulletSpawnPoint;

    private GameObject AudioManager;
    private BulletAudio _audioManager;
    private GameBehaviour gameBehaviour;

    void Start()
    {
        gameBehaviour = FindAnyObjectByType<GameBehaviour>();
        _audioManager = FindObjectOfType<BulletAudio>();
        if (_audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    private void Update()
    {
        fire();
    }

    public void EnemyBulletFire()
    {
        _audioManager = GameObject.Find("AudioManager").GetComponent<BulletAudio>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameBehaviour.onContact();
        }
    }

    public void fire()
    {
        if (GameBehaviour.Instance.State == GameState.Play)
        {
            transform.position += new Vector3(0, -_bulletSpeed, 0) * Time.deltaTime;
        }
    }
}
