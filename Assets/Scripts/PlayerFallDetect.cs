using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDetect : MonoBehaviour
{
    public LevelSpawner levelSpawner;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && levelSpawner.gameStarted == true)
        {
            levelSpawner.LevelFailed(1);
        }
    }
}
