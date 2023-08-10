using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingScript : MonoBehaviour
{
    private GameBehaviour _gameManager;

    private void Start()
    {
        _gameManager = FindAnyObjectByType<GameBehaviour>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            _gameManager.Victory();
        }
    }
}
