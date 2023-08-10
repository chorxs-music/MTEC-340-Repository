using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRefill : MonoBehaviour
{
    private GameBehaviour _gameManager;
    private PlayerShoot _playerShoot;
    [SerializeField] PlayerBehaviour _playerBehaviour;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _refillSound;

    public GameObject Player;

    private void Start()
    {
        _gameManager = FindAnyObjectByType<GameBehaviour>();
        _playerBehaviour = FindAnyObjectByType<PlayerBehaviour>();
        //_playerShoot = FindAnyObjectByType<PlayerShoot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Successfull Collision!");
            PlayerShoot playerShootComponent = Player.GetComponentInChildren<PlayerShoot>();
            _gameManager.RefillBullets();
            StartCoroutine(Destroy());
            if (playerShootComponent != null)
            {
                playerShootComponent.DeclareDeathAbility();
                playerShootComponent.RefillBullets();
            }
            _source.PlayOneShot(_refillSound);
            //_playerBehaviour.RefillBullets();
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
