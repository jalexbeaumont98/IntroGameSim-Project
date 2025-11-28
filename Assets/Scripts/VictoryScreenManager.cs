using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreenManager : MonoBehaviour
{
    void Start()
    {
        AudioController.Instance?.PlayMusic_Victory();
    }

    public void OnContinuePressed()
    {
        AudioController.Instance?.PlaySound_ButtonClick();

        // Load the next level or same level again
        // Replace "MainLevel" with your actual next scene name if available
        SceneManager.LoadScene("MainLevel");
    }

    public void OnReturnToMenuPressed()
    {
        AudioController.Instance?.PlaySound_ButtonClick();
        SceneManager.LoadScene("Title_Screen");
    }
}