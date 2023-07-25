using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonPlay : MonoBehaviour
{
    [SerializeField] AudioClip _playSound;
    public AudioSource _source;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }
        
    public void LoadGameplayScene() 
    {
        _source = GetComponent<AudioSource>();
        _source.PlayOneShot(_playSound);
        SceneManager.LoadScene("Gameplay");
    }

    private void QuitGame()
    {
#if UNITY_EDITOR

        EditorApplication.isPlaying = false;

#else
        Application.Quit();

#endif
    }
}
