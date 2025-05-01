using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public DiarySystem system;
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("E");
            system.IncreasePages();
            Destroy(gameObject);
        }
    }
}
