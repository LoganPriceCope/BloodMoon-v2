using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Player;
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] public AudioSource SFXSource;
    [SerializeField] AudioSource ambienceSource;

    public AudioClip mainMenuBackgroundSong;
    public AudioClip playSoundEffect;
    public AudioClip fireSound;
    public AudioClip click;
    public AudioClip hover;
    public AudioClip leaves;
    public AudioClip breathingHard;
    public AudioClip heartBeat;

    public AudioClip Footsteps1;
    public AudioClip Footsteps2;
    public AudioClip Footsteps3;

    private PlayerMovement playerMovement;

    public static AudioManager instance;



    private void Awake()
    {
        if (instance == null)
        {
            // if instance is null, store a reference to this instance
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Another instance of this gameobject has been made so destroy it
            // as we already have one
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        
        GameObject obj = GameObject.FindGameObjectWithTag("Enemy");
        if (obj == null)
        {
        }
        else
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            musicSource.clip = mainMenuBackgroundSong;
            musicSource.Play();
            ambienceSource.clip = fireSound;
            ambienceSource.Play();
        }
        else if (SceneManager.GetActiveScene().name == "MainGame")
        {
            musicSource.volume = 0;
            ambienceSource.volume = 0;
        }
    }
    public void Update()
    {
    }
    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
        // To use this do:
        // AudioManager audioManager;
        // private void Awake(){audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();}
        // audioManager.PlaySFX(audioManager.AUDIOCLIP);
    }
    public void ChangeAmbienceVolume(float volume)
    {
        ambienceSource.volume = volume;
    }
    public void ChangeMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void ChangeSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }

    public void ReturnMusicValues(out float music)
    {
        music = musicSource.volume;
    }

    public void ReturnSFXValues(out float sfx)
    {
        sfx = SFXSource.volume;
    }

}
