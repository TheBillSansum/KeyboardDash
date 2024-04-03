using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPush : MonoBehaviour
{
    public float pushForce = 10;
    public Direction conveyorDirection_;
    public Rigidbody rb;
    public GameObject parent;
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private void OnTriggerEnter(Collider other)
    {
        rb = other.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        rb.AddForce(pushForce * parent.transform.right, ForceMode.Force);
    }
}
