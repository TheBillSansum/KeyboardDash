using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEvents : MonoBehaviour
{
    public GameObject objectToSpawn;

    public void StartGame()
    {
        objectToSpawn.SetActive(true);
        Debug.Log("Started" + this.gameObject.name);

    }

}
