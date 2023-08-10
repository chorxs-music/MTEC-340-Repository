using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    float _speed = 15.0f;
    private GameBehaviour _gameManager;

    private void Update()
    {
        if (GameBehaviour.Instance.State == GameStates.GameState.Play)
        {
            Rigidbody bulletRb = GetComponent<Rigidbody>();
            bulletRb.velocity = transform.forward * _speed;

            Destroy(gameObject, 3.0f);
        }
    }
}
