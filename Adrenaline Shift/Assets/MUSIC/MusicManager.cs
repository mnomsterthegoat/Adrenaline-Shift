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
        Debug.Log("Scene Loaded: " + scene.name); // Debug statement
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

        Debug.Log("Clip to play: " + (clipToPlay != null ? clipToPlay.name : "null")); // Debug statement

        if (clipToPlay != null)
        {
            if (audioSource.clip == clipToPlay && audioSource.isPlaying)
            {
                // Do nothing, the correct music is already playing
                Debug.Log("Music is already playing: " + clipToPlay.name); // Debug statement
                return;
            }
            else
            {
                audioSource.clip = clipToPlay;
                audioSource.Play();
                Debug.Log("Playing music: " + clipToPlay.name); // Debug statement
            }
        }
        else
        {
            // Stop the music if there is no music for the current scene
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("Stopping music"); // Debug statement
            }
        }
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume; // Adjust the volume of the music
        Debug.Log("Volume set to: " + volume); // Debug statement
    }
}
