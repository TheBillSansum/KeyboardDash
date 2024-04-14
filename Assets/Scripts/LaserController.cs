using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Move the laser object back and forth between the startPoint and endPoint and detect if the player collides with it.
/// <para> Can also be utilised for non-moving lasers by settings speed to 0 </para>
/// </summary>
public class LaserController : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint; 
    public float speed = 2f; //Movement speed, set to 0 to have non-moving lasers
    public LevelSpawner levelSpawner;

    public void Start()
    {
        levelSpawner = GameObject.FindAnyObjectByType<LevelSpawner>();
    }

    private void FixedUpdate()
    {
        if (speed != 0) //If the laser should move
        {
            float pingPong = Mathf.PingPong(Time.time * speed, 1f); //Move back and forth between the two points
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, pingPong);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levelSpawner.gameStarted == true) //If the player collides and the game has already started (Ensuring only runs once) 
        {
            levelSpawner.LevelFailed(2); //Run the 'Hit by Laser' fail message
        }
    }
}