using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Functionality for firing cannon balls, holds a list of transforms to fire from which it picks from randomly
/// </summary>
public class Cannon : MonoBehaviour
{
    public GameObject cannonballPrefab; // Prefab of the cannonball
    public List<Transform> firePoints; // Points from where the cannonball will be fired
    public Transform target; // Target object to fire towards
    public float shootForce = 1000f; // Force applied to the cannonball
    public float minShootInterval = .1f; // Minimum time between shots
    public float maxShootInterval = .1f; // Maximum time between shots
    public bool stopFiring = false;

    private List<GameObject> cannonballs = new List<GameObject>(); // List to store fired cannonballs
    private float timer; // Timer to control shooting intervals
    private float nextShootTime; // Time for the next shoot

    void Start()
    {
        nextShootTime = Random.Range(minShootInterval, maxShootInterval);
    }

    void FixedUpdate()
    {
        if (stopFiring != true)
        {
            timer += Time.deltaTime;
            if (timer >= nextShootTime)
            {
                ShootCannonball();
                timer = 0f;
                nextShootTime = Random.Range(minShootInterval, maxShootInterval);
            }
        }
    }

    /// <summary>
    /// Shoots a singular cannon ball at a desired location
    /// </summary>
    void ShootCannonball()
    {
        Transform selectedFirePoint = firePoints[Random.Range(0, firePoints.Count)]; //Pick a random location to fire from
        GameObject cannonball = Instantiate(cannonballPrefab, selectedFirePoint.position, selectedFirePoint.rotation); //Save to a gameobject to be added to a list
        cannonballs.Add(cannonball); //Add to the list for later deletion

        Vector3 targetDirection = Vector3.left;

        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(targetDirection * shootForce);
        }
    }
    /// <summary>
    /// Despawn all cannon balls instantly
    /// </summary>
    public void DisposeAllCannonballs()
    {
        foreach (var cannonball in cannonballs)
        {
            Destroy(cannonball); 
        }
        stopFiring = true;
        cannonballs.Clear();
    }
}
