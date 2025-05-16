using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ArrowScript : MonoBehaviour
{
    public List<Transform> regularPickups = new List<Transform>(); // pickups in any order
    public Transform finalPickup; // assign the final/special pickup manually in Inspector
    public float rotationSpeed = 5f;

    private Transform currentTarget;

    void Update()
    {
        // Decide what to look at
        if (regularPickups.Count > 0)
        {
            currentTarget = GetClosestTarget(regularPickups);
        }
        else if (finalPickup != null)
        {
            currentTarget = finalPickup;
        }
        else
        {
            currentTarget = null; // Nothing to look at
        }

        // Rotate toward the current target
        if (currentTarget != null)
        {
            Vector3 direction = currentTarget.position - transform.position;
            direction.y = 0; // Keep rotation flat

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                // Optional: adjust this if arrow model doesn’t face Z+:
                Quaternion offset = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation * offset, Time.deltaTime * rotationSpeed);
            }
        }
    }

    public void RegisterPickup(Transform collected)
    {
        if (regularPickups.Contains(collected))
        {
            regularPickups.Remove(collected);
        }
        else if (collected == finalPickup)
        {
            finalPickup = null;
        }
    }

    private Transform GetClosestTarget(List<Transform> targets)
    {
        Transform closest = null;
        float minDist = float.MaxValue;

        foreach (Transform t in targets)
        {
            if (t == null) continue;

            float dist = Vector3.Distance(transform.position, t.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = t;
            }
        }

        return closest;
    }
}
