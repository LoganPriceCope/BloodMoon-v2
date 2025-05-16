using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject playButtonEffect;
    public GameObject playButton;

    public GameObject settingsButtonEffect;
    public GameObject settingsButton;

    public GameObject quitButtonEffect;
    public GameObject quitButton;

    public GameObject tutorialButton;


    public GameObject tutorialMenu;
    public GameObject settingsMenu;

    private Vector3 originalButtonScale;
    private Vector3 ButtonScaleAfter;

    public Animator playerAnimator;
    private Vignette mainVignette;
    public Volume volume;
    public GameObject fadeUI;
    public Image fade;
    public bool inFade = false;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        originalButtonScale = playButton.transform.localScale;
        ButtonScaleAfter = originalButtonScale + new Vector3(0.05f, 0.05f, 0.05f);

        
    }

    public void Play()
    {
        audioManager.PlaySFX(audioManager.click);
        SettingsClose();
        playerAnimator.SetBool("Play", true);
        if (volume.profile.TryGet(out mainVignette))
        {
            LeanTween.value(mainVignette.intensity.value, 1f, 2f)
                .setOnUpdate((float val) =>
                {
                    mainVignette.intensity.value = val;
                if (inFade == false)
                {
                        StartCoroutine(Fade());
                }  
                });
        }
    }
    private IEnumerator Fade()
    {
        inFade = true;
        yield return new WaitForSeconds(0.5f);
        fadeUI.SetActive(true);
        LeanTween.value(fade.color.a, 1f, 1.5f)
            .setOnUpdate((float val) =>
            {
                Color c = fade.color;
                c.a = val; 
                fade.color = c;
                StartCoroutine(ChangeScene());
            });
    }

    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ScalePlayButtonStart()
    {
        audioManager.PlaySFX(audioManager.hover);
        LeanTween.scaleX(playButtonEffect, 96.65f, 0.12f);
        LeanTween.moveLocalX(playButtonEffect, 174f, 0.12f);

        LeanTween.scale(playButton, ButtonScaleAfter, 0.12f);
    }

    public void ScalePlayButtonEnd()
    {
            LeanTween.scaleX(playButtonEffect, 9f, 0.12f);
            LeanTween.moveLocalX(playButtonEffect, 158f, 0.12f);

            LeanTween.scale(playButton, originalButtonScale, 0.12f);

    }

    public void ScaleSettingsButtonStart()
    {
        audioManager.PlaySFX(audioManager.hover);
        LeanTween.scaleX(settingsButtonEffect, 96.65f, 0.12f);
        LeanTween.moveLocalX(settingsButtonEffect, 174f, 0.12f);

        LeanTween.scale(settingsButton, ButtonScaleAfter, 0.12f);
    }

    public void ScaleSettingsButtonEnd()
    {
        LeanTween.scaleX(settingsButtonEffect, 9f, 0.12f);
        LeanTween.moveLocalX(settingsButtonEffect, 158f, 0.12f);

        LeanTween.scale(settingsButton, originalButtonScale, 0.12f);

    }

    public void ScaleQuitButtonStart()
    {
        audioManager.PlaySFX(audioManager.hover);
        LeanTween.scaleX(quitButtonEffect, 96.65f, 0.12f);
        LeanTween.moveLocalX(quitButtonEffect, 174f, 0.12f);

        LeanTween.scale(quitButton, ButtonScaleAfter, 0.12f);
    }

    public void ScaleQuitButtonEnd()
    {
        LeanTween.scaleX(quitButtonEffect, 9f, 0.12f);
        LeanTween.moveLocalX(quitButtonEffect, 158f, 0.12f);

        LeanTween.scale(quitButton, originalButtonScale, 0.12f);
    }

    public void ScaleTutorialButtonStart()
    {
        audioManager.PlaySFX(audioManager.hover);

        LeanTween.scale(tutorialButton, ButtonScaleAfter, 0.12f);
    }

    public void ScaleTutorialButtonEnd()
    {

        LeanTween.scale(tutorialButton, originalButtonScale, 0.12f);
    }

    public void SettingsOpen()
    {
        audioManager.PlaySFX(audioManager.click);
        LeanTween.moveLocalY(settingsMenu, -28f, 0.3f);
    }

    public void SettingsClose()
    {
        audioManager.PlaySFX(audioManager.click);
        LeanTween.moveLocalY(settingsMenu, 902f, 0.3f);
    }

    public void TutorialOpen()
    {
        audioManager.PlaySFX(audioManager.click);
        LeanTween.moveLocalY(tutorialMenu, -28f, 0.3f);
    }

    public void TutorialClose()
    {
        audioManager.PlaySFX(audioManager.click);
        LeanTween.moveLocalY(tutorialMenu, 902f, 0.3f);
    }
}
