using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Functionality for the cannon balls to auto despawn after set time
/// </summary>


public class SelfDespawn : MonoBehaviour
{
    public float lifetime;

    void Update()
    {
        lifetime -= Time.deltaTime;
        if(lifetime <= 0)
        {
            Destroy(this);
        }
    }
}
