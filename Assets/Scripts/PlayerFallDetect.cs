using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If the key falls off the board.
/// <para>Also can detect if the player falls on the bottom trigger or top trigger to display different messages to the player</para>
/// </summary>
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
                levelSpawner.LevelFailed(1); //Normal fell off fail message
            }
            else if( location == "Top")
            {
                levelSpawner.LevelFailed(3); //Zero gravity fail message
            }
        }
    }
}
