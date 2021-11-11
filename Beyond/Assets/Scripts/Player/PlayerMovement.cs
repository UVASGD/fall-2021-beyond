﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * By AJ Nye and Eric Weng
 * TODO separate into player manager and sub scripts
 */
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    // Waypoint Fields
    /*
     * Lists consist of all the path, and their child objects
     * should contain all points in order from start to finish.
     */
    [SerializeField] private List<Transform> rails = new List<Transform>();
    private int railIndex;
    private int waypointIndex = 0; // Which point along the rail we are pathing to

    // Movement Parameters
    [SerializeField] private float dashTime = 0.05f; // issue: dash seems slow
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float slowSpeed = 5f;
    [SerializeField] private float slideSpeedInc = 20f;

    // Jump Parameeters
    [SerializeField] private float hangFactor = 0.5f;
    [SerializeField] private float jumpVel = 35f;
    [SerializeField] private float counterJumpForce = -15f;

    // Debugging Flags
    private bool isDashing = false;
    private float currSpeed;

    private bool isGrounded = true;
    private bool isFalling = false;
    private bool hitsWall = false;

    private bool jumpKeyHeld = false;
    private bool slideCooldown = false;
    private bool isSliding = false;

    void Start()
    {
        railIndex = rails.Count / 2; // select the middle rail
        rb = GetComponent<Rigidbody>();
        currSpeed = runSpeed;
    }

    void Update()
    {
        /* Jump */

        //Sets grounded if close enough to ground
        if (!isGrounded && isFalling)
        {
            if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit))
            {
                if (hit.distance <= (GetComponent<Collider>().bounds.size.y / 2) + hangFactor)
                {
                    isGrounded = true;
                }
            }
        }

        if (rb.velocity.y < 0)
        {
            isFalling = true;
        }

        //Sets initial jump force
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyHeld = true;
            if (isGrounded && isFalling && !isSliding)
            {
                isGrounded = false;
                isFalling = false;

                Vector3 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;

                rb.AddForce(transform.up * jumpVel, ForceMode.Impulse);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpKeyHeld = false;
        }

        //Adds counter jump force to make a "smaller" jump when space not held
        if(!isGrounded && !jumpKeyHeld)
        {
            rb.AddForce(counterJumpForce * transform.up * rb.mass);
        }

        /* Move Along Rail */

        //Chooses the rail to go towards as nextPoint
        Vector3 nextPoint = rails[railIndex].GetChild(waypointIndex).position;
                
        //Sets y the same as transform as y does not matter
        nextPoint.y = transform.position.y;
        Vector3 pos = transform.position;

        //Direction vector used for direction for dashing
        Vector3 dashDirection = (transform.position - nextPoint).normalized;

        // Move our position a step closer to the target.
        float step = currSpeed * Time.deltaTime; // calculate distance to move

        /* Wall Check */

        //Checks head and feet to not move if it detects a wall
        if (hitsWall)
        {
            Vector3 feetPos = transform.position;
            feetPos.y -= 0.6f;
            Vector3 headPos = transform.position;
            headPos.y += 0.6f;

            if ((Physics.Raycast(feetPos, transform.forward * -1, out RaycastHit feetHit) && feetHit.distance <= 1)
                || (Physics.Raycast(headPos, transform.forward * -1, out RaycastHit headHit) && headHit.distance <= 1))
            {
                hitsWall = true;
            }
            else
            {
                hitsWall = false;
                //transform.position = Vector3.MoveTowards(pos, pointAdj, step);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(pos, nextPoint, step);
        }


        // Check if the position of the cube and sphere are approximately equal.
        // Then sets next point to go to
        if (Vector3.Distance(pos, nextPoint) < 1f)
        {
            if (++waypointIndex >= rails[railIndex].childCount)
                waypointIndex--;
        }

        /* Slide */

        if (Input.GetKey(KeyCode.LeftShift) && !isSliding)
        {
            currSpeed = slowSpeed;
        }
        else if (!isSliding)
        {
            currSpeed = runSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !slideCooldown && isGrounded)
        {
            StartCoroutine(slide());
        }

        /* Dash */

        
        //Debug.Log(waypointIndex);

        //Dashes LEFT or RIGHT to change rails
        if (Input.GetKeyDown(KeyCode.A) && !isDashing && !isSliding && isGrounded)
        {
            if (--railIndex < 0) // we are already at left edge
            {
                railIndex = 0;                
            }
            else
            {
                StartCoroutine(dashWait());
                //StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, 0)));

            }
            Debug.Log(railIndex);
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isDashing && !isSliding && isGrounded)
        {
            if (++railIndex >= rails.Count) // we are already at right edge
            {
                railIndex = rails.Count - 1;                
            }
            else
            {
                StartCoroutine(dashWait());
                //StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, -2)));
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, 0)));

            }
            Debug.Log(railIndex);
        }
    }

    IEnumerator dashWait()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    IEnumerator dash(Vector3 finalPos)
    {
        float elapsedTime = 0;
        float waitTime = dashTime;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(transform.position, finalPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        transform.position = finalPos;
        yield return null;
    }

    IEnumerator slide()
    {
        isSliding = true;
        slideCooldown = true;
        for (int x = 10; x > 0; x--)
        {
            transform.localScale = new Vector3(1, 1 + x * 0.1f, 1);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = new Vector3(1, 1, 1);
        currSpeed = slideSpeedInc;
        yield return new WaitForSeconds(0.5f);
        for (int x = 0; x < 10; x++)
        {
            transform.localScale = new Vector3(1, 1 + x * 0.1f, 1);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = new Vector3(1, 2, 1);
        currSpeed -= slideSpeedInc;
        isSliding = false;
        yield return new WaitForSeconds(0.1f);
        slideCooldown = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 feetPos = transform.position;
        feetPos.y -= 0.5f;
        Vector3 headPos = transform.position;
        headPos.y += 0.5f;

        if ((Physics.Raycast(feetPos, transform.forward * -1, out RaycastHit feetHit) && feetHit.distance <= 1)
            || (Physics.Raycast(headPos, transform.forward * -1, out RaycastHit headHit) && headHit.distance <= 1))
        {
            hitsWall = true;
        }
        else
        {
            hitsWall = false;
        }
    }
}
