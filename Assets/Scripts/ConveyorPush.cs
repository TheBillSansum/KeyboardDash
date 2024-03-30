using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorPush : MonoBehaviour
{
    public float pushForce = 10;
    public Direction conveyorDirection_;
    public Rigidbody rb;
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
        Vector3 forceDirection = Vector3.zero;
        switch (conveyorDirection_)
        {
            case Direction.Left:
                forceDirection = transform.right;
                break;
            case Direction.Right:
                forceDirection = -transform.right;
                break;
            case Direction.Up:
                forceDirection = -transform.up;
                break;
            case Direction.Down:
                forceDirection = transform.up;
                break;
        }

        rb.AddForce(pushForce * forceDirection, ForceMode.Force);
    }
}
