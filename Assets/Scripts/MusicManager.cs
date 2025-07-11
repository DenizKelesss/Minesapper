// Assets/Scripts/MusicManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainMenuMusic;
    public AudioClip inGameMusic;

    [SerializeField]
    private AudioSource audioSource;
    private static MusicManager instance;

    void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if(!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        PlayMainMenuMusic();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1" || scene.name == "Level2" || scene.name == "Level3" || scene.name == "Level4") // In the future, just rewrite this to != "Main Menu" and != "Whatever else"
        {
            PlayInGameMusic();
        }
        else if (scene.name == "Main Menu")
        {
            PlayMainMenuMusic();
        }
    }

    void PlayMainMenuMusic()
    {
        if (audioSource.clip != mainMenuMusic)
        {
            audioSource.clip = mainMenuMusic;
            audioSource.Play();
        }
    }

    void PlayInGameMusic()
    {
        if (audioSource.clip != inGameMusic)
        {
            audioSource.clip = inGameMusic;
            audioSource.Play();
        }
    }
}