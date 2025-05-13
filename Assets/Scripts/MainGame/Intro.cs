using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    AudioManager audioManager;

    public PlayerMovement playerMovementScript;
    public MouseLook mouseLookScript;
    public GameObject player;
    public GameObject canvas;


    public Image fade;
    public GameObject text;

    float volume = 0f;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        player.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        canvas.SetActive(true);
    }

    void Start()
    {
        playerMovementScript.speed = 0f;
        playerMovementScript.gravity = 0f;
        mouseLookScript.mouseSensitivity = 0f;
        audioManager.ChangeAmbienceVolume(0f);
        StartCoroutine(VolumeFade());
    }

    void Update()
    {
        
    }
   
    IEnumerator VolumeFade()
    {
        StartCoroutine(Fade());
        StartCoroutine(RotatePlayer());
        float currentTime = 9f;
        float start;
        audioManager.ReturnMusicValues(out start);
        while(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            Console.WriteLine(currentTime);
            audioManager.ChangeMusicVolume(currentTime / 3);

            yield return null;
        }
        
        yield break;
    }

    private IEnumerator Fade()
    {
        audioManager.PlaySFX(audioManager.leaves);
        yield return new WaitForSeconds(1.5f);
        LeanTween.value(fade.color.a, 0f, 10f)
            .setOnUpdate((float val) =>
            {
                Color c = fade.color;
                c.a = val;
                fade.color = c;
            });
        text.active = false;
    }

    private IEnumerator RotatePlayer()
    {
        yield return new WaitForSeconds(4f);
        LeanTween.rotateX(player, 0f, 2f);
        StartCoroutine(EndCutscene());

    }

    private IEnumerator EndCutscene()
    {
        yield return new WaitForSeconds(3f);
        playerMovementScript.speed = 3f;
        playerMovementScript.gravity = -19.62f;
        mouseLookScript.mouseSensitivity = 150f;
        canvas.SetActive(false);
    }
}
