using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class DiarySystem : MonoBehaviour
{
    public AudioSource beastSoundSource;
    public AudioClip beastSoundClip;
    AudioManager audioManager;
    public int pages = 0;
    int totalPages = 10;
    public GameObject counterTextObject;
    public Text secondaryCounterObject;
    public GameObject beastTextObject;
    public Text counterText;

    // BlackedText
    public GameObject redacted1;
    public GameObject redacted2;
    public GameObject redacted3;
    public GameObject redacted4;
    public GameObject redacted5;
    public GameObject redacted6;
    public GameObject redacted7;
    public GameObject redacted8;
    public GameObject redacted9;
    public GameObject redacted10;

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {

    }

    void Update()
    {
        print(pages);
        counterText.text = pages + "/" + totalPages + " Pages";
        secondaryCounterObject.text = pages + "/" + totalPages + " Pages Acquired";
    }

    public void IncreasePages()
    {
        pages++;
        if (pages == 1)
        {
            redacted1.SetActive(false);
        }
        if (pages == 2)
        {
            redacted2.SetActive(false);
        }
        if (pages == 3)
        {
            redacted3.SetActive(false);
        }
        if (pages == 4)
        {
            redacted4.SetActive(false);
        }
        if (pages == 5)
        {
            redacted5.SetActive(false);
        }
        if (pages == 6)
        {
            redacted6.SetActive(false);
        }
        if (pages == 7)
        {
            redacted7.SetActive(false);
        }
        if (pages == 8)
        {
            redacted8.SetActive(false);
        }
        if (pages == 9)
        {
            redacted9.SetActive(false);
        }
        if (pages == 10)
        {
            redacted10.SetActive(false);
        }
        StartCoroutine(UICooldown());
    }

    IEnumerator UICooldown()
    {
        beastSoundSource.PlayOneShot(beastSoundClip);
        audioManager.PlaySFX(audioManager.leaves);
        beastTextObject.SetActive(true);
        counterTextObject.SetActive(true);
        yield return new WaitForSeconds(3);
        counterTextObject.SetActive(false);
        beastTextObject.SetActive(false);
    }
}
