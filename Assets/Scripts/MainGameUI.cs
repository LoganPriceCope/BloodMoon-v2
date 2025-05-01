using UnityEngine;

public class MainGameUI : MonoBehaviour
{
    public GameObject Diary;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Diary.active == false)
            {
                Diary.SetActive(true);
            }
            else
            {
                Diary.SetActive(false);
            }
        }

    }
}
