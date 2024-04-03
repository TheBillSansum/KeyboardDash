using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void ShootCannonball()
    {
        Transform selectedFirePoint = firePoints[Random.Range(0, firePoints.Count)];
        GameObject cannonball = Instantiate(cannonballPrefab, selectedFirePoint.position, selectedFirePoint.rotation);
        cannonballs.Add(cannonball);

        Vector3 targetDirection = Vector3.left;

        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(targetDirection * shootForce);
        }
    }
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
