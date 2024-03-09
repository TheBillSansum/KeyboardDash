using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPush : MonoBehaviour
{
    public float pushForce = 10;
    public Rigidbody rb;


    private void OnTriggerEnter(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        rb.AddRelativeForce(0, 0, pushForce);
    }
}
