using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * By AJ Nye and Eric Weng
 */
public class PlayerDashSlide : MonoBehaviour
{
    // Waypoint Fields
    // All points on all three paths: left, middle, right
    [SerializeField] private List<Transform> leftPoints = new List<Transform>();
    [SerializeField] private List<Transform> middlePoints = new List<Transform>();
    [SerializeField] private List<Transform> rightPoints = new List<Transform>();

    // Movement Parameters
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float runSpeed = 10.0f;
    [SerializeField] private float dashSpeed = 5.0f;

    // Debugging Flags
    private int waypointIndex = 0; // At which point it is at in its pathing
    private Direction rail = Direction.MIDDLE;

    private bool isDashing = false;
    private float currSpeed;
    private bool slideCooldown = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currSpeed = runSpeed;
    }

    void Update()
    {
        /* Move Along Rail */

        Vector3 nextWaypoint;

        //Chooses the rail to go towards as pointAdj
        if (rail == Direction.MIDDLE)
        {
            nextWaypoint = middlePoints[waypointIndex].position;
        }
        else if (rail == Direction.LEFT)
        {
            nextWaypoint = leftPoints[waypointIndex].position;
        }
        else
        {
            nextWaypoint = rightPoints[waypointIndex].position;
        }

        //Sets y the same as transform as y does not matter
        nextWaypoint.y = transform.position.y;
        Vector3 pos = transform.position;

        //Direction vector used for direction for dashing
        Vector3 dashDirection = (transform.position - nextWaypoint).normalized;

        // Move our position a step closer to the target.
        float step = currSpeed * Time.deltaTime; // calculate distance to move

        /* Wall Check */

        Vector3 feetPos = transform.position;
        feetPos.y -= 0.5f;
        Vector3 headPos = transform.position;
        headPos.y += 0.5f;

        // Checks head and feet to not move if it detects a wall
        if ((Physics.Raycast(feetPos, transform.forward * -1, out RaycastHit feetHit) && feetHit.distance <= 1)
            || (Physics.Raycast(headPos, transform.forward * -1, out RaycastHit headHit) && headHit.distance <= 1))
        {
            //Debug.Log("touching wall");
        }
        else
        {
            transform.position = Vector3.MoveTowards(pos, nextWaypoint, step);
        }

        // Check if the position of the cube and sphere are approximately equal.
        // Then sets next point to go to
        if (Vector3.Distance(pos, nextWaypoint) < 1f)
        {
            waypointIndex++;
        }

        /* Slide */

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currSpeed = dashSpeed;
        }
        else
        {
            currSpeed = runSpeed;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && !slideCooldown)
        {
            StartCoroutine(Slide());
        }

        /* Dash */

        //Dashes left and right to change rails
        if (Input.GetKeyDown(KeyCode.A) && !isDashing)
        {

            if (rail == Direction.RIGHT)
            {
                rail = Direction.MIDDLE;
                StartCoroutine(DashWait());
                StartCoroutine(Dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));
            }
            else if (rail == Direction.MIDDLE)
            {
                rail = Direction.LEFT;
                StartCoroutine(DashWait());
                StartCoroutine(Dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));

            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isDashing)
        {
            if (rail == Direction.LEFT)
            {
                rail = Direction.MIDDLE;
                StartCoroutine(DashWait());
                StartCoroutine(Dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, -2)));
            }
            else if (rail == Direction.MIDDLE)
            {
                rail = Direction.RIGHT;
                StartCoroutine(DashWait());
                StartCoroutine(Dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, -2)));
            }
        }

    }

    private IEnumerator DashWait()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }

    private IEnumerator Dash(Vector3 finalPos)
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

    private IEnumerator Slide()
    {
        for (int x = 10; x > 0; x--)
        {
            transform.localScale = new Vector3(1, 1 + x * 0.1f, 1);
            yield return new WaitForSeconds(0.001f);
        }
        slideCooldown = true;
        transform.localScale = new Vector3(1, 1, 1);
        currSpeed += 10f;
        yield return new WaitForSeconds(0.5f);
        for (int x = 0; x < 10; x++)
        {
            transform.localScale = new Vector3(1, 1 + x * 0.1f, 1);
            yield return new WaitForSeconds(0.001f);
        }
        transform.localScale = new Vector3(1, 2, 1);
        currSpeed -= 10f;
        yield return new WaitForSeconds(0.1f);
        slideCooldown = false;
    }
}
