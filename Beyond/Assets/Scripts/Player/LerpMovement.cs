using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * By AJ Nye
 */
public class LerpMovement : MonoBehaviour
{
    // Waypoint Fields
    // All points on all three paths: left, middle, right
    [SerializeField] private List<Transform> leftPoints = new List<Transform>();
    [SerializeField] private List<Transform> middlePoints = new List<Transform>();
    [SerializeField] private List<Transform> rightPoints = new List<Transform>();

    public int waypointIndex = 0; // At which point it is at in its pathing
    public Direction rail = Direction.MIDDLE;

    // Movement Parameters
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float hangFactor = 1.5f;
    [SerializeField] private float jumpVel = 15f;
    [SerializeField] private float counterJumpForce = -5f;

    // Movement Flags for Debugging
    public bool isDashing = false;
    public bool isGrounded = true;
    public bool isFalling;
    public bool jumpKeyHeld = false;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    private void Update()
    {

        // Sets grounded if close enough to ground
        if (!isGrounded && isFalling)
        {
            // Check if touching ground from above
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

        // Sets initial jump force
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyHeld = true;
            if (isGrounded && isFalling)
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

        //Dashes left and right to change rails
        if (Input.GetKeyDown(KeyCode.A) && !isDashing)
        {

            if (rail == Direction.RIGHT)
            {
                rail = Direction.MIDDLE;
                //transform.Translate(-5, 0, 0);
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(5, 0, -2)));
            }
            else if (rail == Direction.MIDDLE)
            {
                rail = Direction.LEFT;
                //transform.Translate(-5, 0, 0);
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(5, 0, -2)));
                //Debug.Log("DASH");

            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isDashing)
        {
            if (rail == Direction.LEFT)
            {
                rail = Direction.MIDDLE;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(-5, 0, -2)));
            }
            else if (rail == Direction.MIDDLE)
            {
                rail = Direction.RIGHT;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(-5, 0, -2)));
            }
        }

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

        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move

        Vector3 feetPos = transform.position;
        feetPos.y -= 0.25f;
        if (Physics.Raycast(feetPos, transform.forward * -1, out RaycastHit fwdHit) && fwdHit.distance <= 1)
        {
            //Debug.Log("don't move");
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
}
