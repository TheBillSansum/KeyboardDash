using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallDetect : MonoBehaviour
{
    public LevelSpawner levelSpawner;
    public string location;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && levelSpawner.gameStarted == true)
        {
            if (location == "Bottom")
            {
                levelSpawner.LevelFailed(1);
            }
            else if( location == "Top")
            {
                levelSpawner.LevelFailed(3);
            }
        }
    }
}
