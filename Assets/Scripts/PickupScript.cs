using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public DiarySystem system;
    public ArrowScript arrowScript;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            arrowScript.RegisterPickup(transform);
            system.IncreasePages();
            Destroy(gameObject);
        }
    }
}
