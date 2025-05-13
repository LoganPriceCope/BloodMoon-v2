using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    // Objects
    public Animator playerAnimator;
    public CharacterController controller;
    public Volume volume;
    public Transform groundCheck;
    AudioManager audioManager;
    private Vignette mainVignette;
    public LayerMask groundMask;

    public GameObject playerCamera;
    public GameObject cutsceneCamera;
    public GameObject blackOut;

    // Player Settings
    public float speed; // Player Current Speed
    public float originalSpeed = 4f; // Default Walk Speed
    public float crouchSpeed = 1.5f; // Default Crouch Speed
    public float runSpeed = 10f; // Default Run Speed
    public float gravity = -9.81f;
    public float groundDistance = 0.1f;
    public float jumpHeight = 3f;

    public float playerHeight = 2.5f;
    public float crouchHeight = 1.5f;

    public float staminaBar = 0f;

    public float health = 2f;
    public GameObject bloodEffect;

    // Checks
    public bool isRunning = false;
    public bool isCrouching = false;
    public bool isWalking = false;
    bool isCrouchingSoundMuffled = false;
    bool isGrounded;
    bool canRun = true;
    bool runSoundPlayed = false;
    bool inFade = false;


    // Other Variables
    private Vector3 lastPosition;
    Vector3 velocity;
    

    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        // Gravity
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Walking Check
        if(Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            isWalking = true;
            playerAnimator.SetBool("Walking", true);
        }
        else
        {
            isWalking = false;
            playerAnimator.SetBool("Walking", false);
        }
        lastPosition = transform.position;

        // Crouching
        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerAnimator.SetBool("Crouching", true);
            playerAnimator.SetBool("Walking", false);
            speed = crouchSpeed;
            isRunning = false;
            isCrouching = true;
            controller.height = crouchHeight;
        }
        else
        {
            playerAnimator.SetBool("Crouching", false);
            isCrouching = false;
            controller.height = playerHeight;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {

            audioManager.ChangeSFXVolume(audioManager.SFXSource.volume / 4);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {

            audioManager.ChangeSFXVolume(audioManager.SFXSource.volume * 4);
        }
        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            speed = runSpeed;
            isRunning = true;
            isCrouching = false;
            audioManager.SFXSource.pitch = 1.32f;
        }
        else if (!isCrouching)
        {
            speed = originalSpeed;
            isRunning = false;
            audioManager.SFXSource.pitch = 1f;
        }
        
        if (health == 2)
        {
            bloodEffect.SetActive(false);
        }
        else if (health == 1)
        {
            bloodEffect.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
            cutsceneCamera.SetActive(true);
            bloodEffect.SetActive(false);
            Invoke("BlackOut", 1.8f);
        }

        // Stamina
        // (explanation for me: If the stamina bar is above 0, it will subtract 0.2f. However, if the player is running and has less than 100 stamina, stamima will increase by 0.2f.)
        if (isRunning)
        {
            if (staminaBar < 100)
            {
                staminaBar = staminaBar + 0.05f;
            }
        }
        else
        {
            if (staminaBar > 0)
            {
                staminaBar = staminaBar - 0.05f;
            }
        }
        if (volume.profile.TryGet(out mainVignette))
        {
            LeanTween.value(mainVignette.intensity.value, (staminaBar / 1000f) + 0.45f, 0.8f)
                .setOnUpdate((float val) =>
                {
                    mainVignette.intensity.value = val;
                });
        }

        if (staminaBar < 45)
        {
            canRun = true;
            runSoundPlayed = false;
        }
        else
        {
            OutOfStamina();
            
        }
        if (staminaBar > 60)
        {
            canRun = false;
        }

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        // Jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OutOfStamina() // Enables Stamina Sound
    {
        if (!runSoundPlayed)
        {
            runSoundPlayed = true;
            audioManager.PlaySFX(audioManager.breathingHard);
        }
    }

    void BlackOut()
    {
        blackOut.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}