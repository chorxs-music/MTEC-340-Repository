using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerBehaviour : MonoBehaviour
{
    public Transform bulletSpawnPoint;

    [SerializeField] float _destroyerSpeed = 7.5f;

    private float _xLimit = 8.2f;
    private float _yLimit = -4.36f;
    private float minFireRate = 5.0f;
    private float maxFireRate = 10.0f;
    private BulletAudio _audioManager;
    public GameState State;
    private GameBehaviour gameBehaviour;
    private EnemyBulletBehaviour enemyBulletBehaviour;
    [SerializeField] AudioClip _bulletFire;
    [SerializeField] GameObject _enemyBullet;

    [SerializeField] AudioSource _source;

    int _xDir = 1;

    void Start()
    {
        enemyBulletBehaviour = FindAnyObjectByType<EnemyBulletBehaviour>();
        if (enemyBulletBehaviour != null)
        {
            Debug.Log("Successful Enemy Bullet Script");
        }

        StartCoroutine(EnemyBulletFire());
    }

    IEnumerator EnemyBulletFire()
    {
        while (State == GameState.Play)
        {
            yield return new WaitForSeconds(Random.Range(minFireRate, maxFireRate));
            fire();
        }
    }

    void Update()
    {
        if (GameBehaviour.Instance.State == GameState.Play)
        {
            transform.position += new Vector3(_destroyerSpeed * _xDir, 0, 0) * Time.deltaTime;

            if (Mathf.Abs(transform.position.x) >= _xLimit)
            {
                _xDir *= -1;
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
                transform.position = new Vector3(transform.position.x > 0 ? _xLimit - 0.1f : -_xLimit + 0.1f, transform.position.y, transform.position.z);
            }

            if (transform.position.y <= _yLimit)
            {
                gameBehaviour = FindObjectOfType<GameBehaviour>();
                gameBehaviour.GameOver();
            }
        }
    }

    void fire()
    {
        if (GameBehaviour.Instance.State == GameState.Play)
        {
            Vector3 BulletSpawnPosition = transform.position + new Vector3(0, 0, 0);
            GameObject EnemyBullet = Instantiate(_enemyBullet, bulletSpawnPoint.position, Quaternion.identity);

            Rigidbody2D instantiatedRigidBody = EnemyBullet.GetComponent<Rigidbody2D>();
            instantiatedRigidBody.simulated = true;
            instantiatedRigidBody.useFullKinematicContacts = true;
            _audioManager = GameObject.Find("AudioManager").GetComponent<BulletAudio>();

            enemyBulletBehaviour = _enemyBullet.GetComponent<EnemyBulletBehaviour>();

            enemyBulletBehaviour.fire();
            _source.PlayOneShot(_bulletFire);
            Debug.Log("EnemyDestroyerFire!");
        }
        
    }
}    