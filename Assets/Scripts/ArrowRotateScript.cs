using Unity.Mathematics;
using UnityEngine;

public class ArrowRotateScript : MonoBehaviour
{
    public GameObject player;
    public float rotationSpeed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.right);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
