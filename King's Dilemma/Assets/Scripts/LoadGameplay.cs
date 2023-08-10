using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameplay : MonoBehaviour
{
    public void LoadGameplayScene()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
