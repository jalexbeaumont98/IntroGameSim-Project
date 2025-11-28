using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    void Start()
    {
        AudioController.Instance?.PlayMusic_GameOver();
    }

    public void RetryGame()
    {
        AudioController.Instance?.PlaySound_ButtonClick();
        SceneManager.LoadScene("MainLevel"); // Replay level
    }

    public void ReturnToMenu()
    {
        AudioController.Instance?.PlaySound_ButtonClick();
        SceneManager.LoadScene("Title_Screen"); // Go back to title
    }
}