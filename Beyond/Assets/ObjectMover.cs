using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Rigidbody rb;
    public Vector3 moveForce;

    public void Move()
    {
        rb.AddForce(moveForce);
    }

}
