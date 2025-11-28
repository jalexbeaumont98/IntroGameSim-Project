using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title_Screen") AudioController.Instance.PlayMusic_Title();
    }
    public void StartGame()
    {
        ScreenFader.Instance.FadeOutAndLoadScene("Player_Building_Scene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}