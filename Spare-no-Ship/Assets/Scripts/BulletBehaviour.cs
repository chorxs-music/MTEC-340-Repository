using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float _bulletSpeed = 10.0f;
    public GameState State;
    private BulletAudio _audioManager;
    private GameBehaviour gameBehaviour;
    [SerializeField] GameObject _playerShip;

    void Start()
    {
        gameBehaviour = FindAnyObjectByType<GameBehaviour>();
        
    }

    void Update()
    {
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("EnemyShip"))
        {
            string shipName = collision.gameObject.name;

            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            _audioManager = FindAnyObjectByType<BulletAudio>();
            _audioManager.shipDestroy();
            if (shipName.StartsWith("Scout"))
            {
                gameBehaviour.ScoutDestroyed();
                Debug.Log("Scout Destroyed!");
            }

            else if (shipName.StartsWith("Destroyer"))
            {
                gameBehaviour.DestroyerDestroyed();
                Debug.Log("Destroyer Destroyed!");
            }
            
        }

        if (collision.gameObject.CompareTag("Missile"))
        {
            Destroy(this.gameObject);
            Debug.Log("Enemy missile is too big to destroy!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                Destroy(this.gameObject);
                Destroy(collision.gameObject);
                _audioManager.bulletDestroy();
            }
        }
    }

    public void Fire()
    {
        if (GameBehaviour.Instance.State == GameState.Play)
        {
            transform.position += new Vector3(0, _bulletSpeed, 0) * Time.deltaTime;
        }
    }
}


