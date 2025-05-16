using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class EquipTorch : MonoBehaviour
{
    public GameObject torchPickup;
    public GameObject realTorch;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            realTorch.SetActive(true);
            Destroy(torchPickup);
        }
    }
}
