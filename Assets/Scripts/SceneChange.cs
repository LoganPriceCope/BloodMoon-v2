using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    public GameObject black;
    public Text tipText;
    public Text winText;
    public GameObject winCutscene;
    float elapsedTime;
    public Text timerObject;
    public Text inkObject;
    public GameObject player;

    public DiarySystem diarySystem;

    // Update is called once per frame
    void Update()
    {
        if (!timerObject.isActiveAndEnabled)
        {
            elapsedTime += Time.deltaTime;

        }
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        if (timerObject.isActiveAndEnabled)
        {
            timerObject.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            inkObject.text = diarySystem.pages + "/10";
        }
        
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void OpenMainMenu()
    {
        print("EEE");
        SceneManager.LoadScene(0);
       
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && diarySystem.pages == 10)
        {
            winCutscene.SetActive(true);
            player.SetActive(false);
            Invoke("WinScreen", 2.8f);
        }
    }

    public void WinScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        black.SetActive(true);
        winText.text = "You Win!";
        tipText.text = "Congratulations! You escaped the beast and are awaiting rescue in the bunker.";
    }
}
