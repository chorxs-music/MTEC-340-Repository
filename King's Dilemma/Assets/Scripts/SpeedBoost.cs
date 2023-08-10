using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    private PlayerBehaviour _playerBehaviour;
    [SerializeField] AudioSource _source;
    [SerializeField] AudioClip _speedSound;

    private void Start()
    {
        _playerBehaviour = FindAnyObjectByType<PlayerBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Successfull Collision with " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player"))
        {
            _playerBehaviour.DoubleSpeed();
            StartCoroutine(Destroy());
            _source.PlayOneShot(_speedSound);
            Debug.Log("Successfull Speed Activation!");
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
