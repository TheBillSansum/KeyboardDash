using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilised in the first laser level in moving the laser back and forth
/// </summary>

public class ImagePingPong : MonoBehaviour
{
    public Transform startPoint; 
    public Transform endPoint; 
    public float speed = 2f;

    private Vector3 currentTarget; 

    void Start()
    {
        currentTarget = endPoint.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentTarget) < 0.01f)
        {
            SwitchTarget();
        }
    }
    /// <summary>
    /// Swap Direction
    /// </summary>
    void SwitchTarget()
    {
        if (currentTarget == startPoint.position)
        {
            currentTarget = endPoint.position; 
        }
        else
        {
            currentTarget = startPoint.position;
        }
    }
}
