using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        AudioController.Instance?.PlayMusic_Title();
    }

    public void StartGame()
    {
        AudioController.Instance?.PlaySound_ButtonClick();
        SceneManager.LoadScene("MainLevel"); // Replace with your gameplay scene name
    }

    public void QuitGame()
    {
        AudioController.Instance?.PlaySound_ButtonClick();
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // So it quits in editor too
#endif
    }
}