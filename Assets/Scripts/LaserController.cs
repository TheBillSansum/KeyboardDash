using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint; 
    public float speed = 2f;
    public LevelSpawner levelSpawner;

    public void Start()
    {
        levelSpawner = GameObject.FindAnyObjectByType<LevelSpawner>();
    }

    private void FixedUpdate()
    {
        if (speed != 0)
        {
            float pingPong = Mathf.PingPong(Time.time * speed, 1f);
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, pingPong);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levelSpawner.gameStarted == true)
        {
            levelSpawner.LevelFailed(2);
        }
    }
}