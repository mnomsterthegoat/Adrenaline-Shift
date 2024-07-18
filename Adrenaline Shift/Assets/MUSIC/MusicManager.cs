using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    public AudioClip map1Music;
    public AudioClip map2Music;
    public AudioClip map3Music;
    private AudioSource audioSource;

    private static MusicManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        DontDestroyOnLoad(gameObject); // Keep the MusicManager when loading new scenes
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the sceneLoaded event
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name); // Play music based on the loaded scene
    }

    void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        switch (sceneName)
        {
            case "MainMenu":
                clipToPlay = mainMenuMusic;
                break;
            case "Map1":
                clipToPlay = map1Music;
                break;
            case "Map2":
                clipToPlay = map2Music;
                break;
            case "Map3":
                clipToPlay = map3Music;
                break;
        }

        if (clipToPlay != null)
        {
            if (audioSource.clip == clipToPlay && audioSource.isPlaying)
            {
                // Do nothing, the correct music is already playing
                return;
            }
            else
            {
                audioSource.clip = clipToPlay;
                audioSource.Play();
            }
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume; // Adjust the volume of the music
    }
}
