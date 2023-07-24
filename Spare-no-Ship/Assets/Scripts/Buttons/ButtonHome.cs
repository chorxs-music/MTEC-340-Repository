using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHome : MonoBehaviour
{
    [SerializeField] AudioClip _homeSound;
    public AudioSource _source;

    public void LoadTitleScene()
    {
        _source = GetComponent<AudioSource>();
        _source.PlayOneShot(_homeSound);
        SceneManager.LoadScene("Title");
    }
}
