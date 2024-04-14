using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPush : MonoBehaviour
{
    public float pushForce = 10;
    public Direction conveyorDirection_;
    public Rigidbody rb;
    public GameObject parent; //The button object to get the correct direction
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
        rb.AddForce(pushForce * parent.transform.right, ForceMode.Force); //Move any object with a rb that enters the small trigger box
    }
}
