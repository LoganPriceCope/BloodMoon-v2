using UnityEngine;

public class BookScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject background;

    //method called by animation event

    public void BookClosed()
    {
        background.SetActive(false);

    }

}
