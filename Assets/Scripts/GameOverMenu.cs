using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void OnContinuePressed()
    {
        if (!string.IsNullOrEmpty(GameSessionManager.LastLevel))
        {
            SceneManager.LoadScene(GameSessionManager.LastLevel);
        }
        else
        {
            Debug.LogWarning("No last level recorded!");
        }
    }

    public void OnQuitPressed()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}