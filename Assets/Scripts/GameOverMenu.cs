using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    void Start()
    {
        // Play game over music when this scene loads
        AudioController.Instance?.PlayMusic_GameOver();
    }

    public void OnContinuePressed()
    {
        AudioController.Instance?.PlaySound_ButtonClick();

        if (!string.IsNullOrEmpty(GameSessionManager.LastLevel))
        {
            SceneManager.LoadScene(GameSessionManager.LastLevel);
        }
        else
        {
            Debug.LogWarning("No last level recorded!");
            SceneManager.LoadScene("Title_Screen");
        }
    }

    public void OnQuitPressed()
    {
        AudioController.Instance?.PlaySound_ButtonClick();

        Debug.Log("Quitting Game");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}