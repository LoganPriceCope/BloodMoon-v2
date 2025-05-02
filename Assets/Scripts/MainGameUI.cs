using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    public Animator bookAnimator;
    public GameObject diary;
    AudioManager audioManager;
    public void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (diary.activeInHierarchy == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                diary.SetActive(true);
                bookAnimator.SetBool("Open", true);
                audioManager.PlaySFX(audioManager.pageFlip);
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                bookAnimator.SetBool("Close", true);
                audioManager.PlaySFX(audioManager.pageFlip);
                //diary.SetActive(false);
            }
        }
    }
}
