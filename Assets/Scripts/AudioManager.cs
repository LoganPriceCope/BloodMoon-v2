using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource ambienceSource;

    public AudioClip mainMenuBackgroundSong;
    public AudioClip playSoundEffect;
    public AudioClip fireSound;
    public AudioClip click;
    public AudioClip hover;
    public AudioClip leaves;
    public AudioClip breathingHard;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
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

    public void ReturnMusicValues(out float music)
    {
        music = musicSource.volume;
    }

}
