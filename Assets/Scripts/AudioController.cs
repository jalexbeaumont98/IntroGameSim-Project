using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("Music Clips")]
    public AudioClip titleMusic;
    public AudioClip gameplayMusic;
    public AudioClip victoryMusic;
    public AudioClip gameOverMusic;

    [Header("Sound Effects")]
    public AudioClip playerShoot;
    public AudioClip enemyDeath;
    public AudioClip buttonClick;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayMusic_Title() => PlayMusic(titleMusic);
    public void PlayMusic_Gameplay() => PlayMusic(gameplayMusic);
    public void PlayMusic_Victory() => PlayMusic(victoryMusic);
    public void PlayMusic_GameOver() => PlayMusic(gameOverMusic);

    public void PlaySound_ButtonClick() => sfxSource.PlayOneShot(buttonClick);
    public void PlaySound_PlayerShoot() => sfxSource.PlayOneShot(playerShoot);
    public void PlaySound_EnemyDeath() => sfxSource.PlayOneShot(enemyDeath);
}