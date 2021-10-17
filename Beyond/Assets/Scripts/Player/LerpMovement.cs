using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMovement : MonoBehaviour
{
    private Rigidbody rb;

    // Waypoint Fields
    //All points on all three paths: left, middle, right
    [SerializeField] private List<Transform> leftPoints = new List<Transform>();
    [SerializeField] private List<Transform> middlePoints = new List<Transform>();
    [SerializeField] private List<Transform> rightPoints = new List<Transform>();

    // Movement Parameters
    // TODO do raycast in player manager
    [SerializeField] private float dashTime = 0.15f;
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

    private int waypointIndex = 0; // At which point it is at in its pathing
    private Direction rail = Direction.MIDDLE;

    private bool isGrounded = true;
    private bool isFalling = false;
    private bool hitsWall = false;

    private bool jumpKeyHeld = false;
    private bool slideCooldown = false;
    private bool isSliding = false;

    void Start()
    {
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

        Vector3 pointAdj;

        //Chooses the rail to go towards as pointAdj
        if (rail == Direction.MIDDLE)
        {
            pointAdj = middlePoints[waypointIndex].position;

        }
        else if (rail == Direction.LEFT)
        {
            pointAdj = leftPoints[waypointIndex].position;
        }
        else
        {
            pointAdj = rightPoints[waypointIndex].position;
        }

        //Sets y the same as transform as y does not matter
        pointAdj.y = transform.position.y;
        Vector3 pos = transform.position;

        //Direction vector used for direction for dashing
        Vector3 dashDirection = (transform.position - pointAdj).normalized;

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
            transform.position = Vector3.MoveTowards(pos, pointAdj, step);
        }


        // Check if the position of the cube and sphere are approximately equal.
        // Then sets next point to go to
        if (Vector3.Distance(pos, pointAdj) < 1f)
        {
            waypointIndex++;
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

        //Dashes LEFT and RIGHT to change rails
        if (Input.GetKeyDown(KeyCode.A) && !isDashing)
        {

            if (rail == Direction.RIGHT)
            {
                rail = Direction.MIDDLE;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));
            }
            else if (rail == Direction.MIDDLE)
            {
                rail = Direction.LEFT;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));

            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isDashing)
        {
            if (rail == Direction.LEFT)
            {
                rail = Direction.MIDDLE;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, -2)));
            }
            else if (rail == Direction.MIDDLE)
            {
                rail = Direction.RIGHT;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, -2)));
            }
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
