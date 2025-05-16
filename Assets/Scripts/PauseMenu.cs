using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject buttons;
    public GameObject mainUI;
    public GameObject introCanvas;
    public GameObject blackScreen;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buttons.activeInHierarchy == false && introCanvas.activeInHierarchy == false && blackScreen.activeInHierarchy == false)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                buttons.SetActive(true);
                mainUI.SetActive(false);
                Time.timeScale = 0;
            }
            /*else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                buttons.SetActive(false);
                mainUI.SetActive(true);
            }*/
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        buttons.SetActive(false);
        mainUI.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ChangeToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }
}
