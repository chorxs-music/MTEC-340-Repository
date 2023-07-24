using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonTutorial : MonoBehaviour
{
    [SerializeField] AudioClip _tutorialSound;
    public AudioSource _source;

    public void LoadTutorialScene()
    {
        _source = GetComponent<AudioSource>();
        _source.PlayOneShot(_tutorialSound);
        SceneManager.LoadScene("Tutorial");
    }
}
