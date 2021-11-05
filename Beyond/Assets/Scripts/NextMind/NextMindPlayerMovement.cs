using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMindPlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 1000f;
    public float sideForce = 600f;
    public float jumpForce = 250f;


    // FixedUpdate is used to mess with physics
    void FixedUpdate()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);

        // Input is better for Update()
        if (Input.GetKey("d"))
        {
            rb.AddForce(sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a"))
        {
            rb.AddForce(-sideForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //    Jump();

    }

    public void SetForwardForce(float f)
    {
        forwardForce = f;
    }

    public void Jump()
    {
        rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
    }
}
