using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dir
{
    left,
    right,
    middle
}

public class LerpMovement : MonoBehaviour
{
    //All points on all three paths: left, middle, right
    public List<Transform> leftPoints = new List<Transform>();
    public List<Transform> middlePoints = new List<Transform>();
    public List<Transform> rightPoints = new List<Transform>();

    public bool isDashing = false;
    public float dashTime = 0.15f;
    public float runSpeed = 10.0f;
    public float slowSpeed = 5f;
    public float slideSpeedInc = 20f;

    private float currSpeed;

    //At which point it is at in its pathing
    public int index = 0;
    public dir rail = dir.middle;

    public bool isGrounded = true;
    public bool isFalling = false;
    public bool hitsWall = false;

    public float hangFactor = 0.5f;
    public float jumpVel = 35f;
    public float counterJumpForce = -15f;

    private Rigidbody rb;

    public bool jumpKeyHeld = false;

    private bool slideCooldown = false;
    private bool isSliding = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        currSpeed = runSpeed;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
    }

    // Update is called once per frame
    void Update()
    {
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



        Vector3 pointAdj;

        //Chooses the rail to go towards as pointAdj
        if (rail == dir.middle)
        {
            pointAdj = middlePoints[index].position;

        }
        else if (rail == dir.left)
        {
            pointAdj = leftPoints[index].position;
        }
        else
        {
            pointAdj = rightPoints[index].position;
        }

        //Sets y the same as transform as y does not matter
        pointAdj.y = transform.position.y;
        Vector3 pos = transform.position;

        //Direction vector used for direction for dashing
        Vector3 dashDirection = (transform.position - pointAdj).normalized;

        // Move our position a step closer to the target.
        float step = currSpeed * Time.deltaTime; // calculate distance to move

        

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
            index++;
            if (index == middlePoints.Count)
            {
                index = 0;
                if (rail == dir.left)
                {
                    transform.position = leftPoints[index].position;
                }
                else if (rail == dir.middle)
                {
                    transform.position = middlePoints[index].position;
                }
                else
                {
                    transform.position = rightPoints[index].position;
                }
            }
        }

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

        //Dashes left and right to change rails
        if (Input.GetKeyDown(KeyCode.A) && !isDashing && !isSliding && isGrounded)
        {

            if (rail == dir.right)
            {
                rail = dir.middle;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));
            }
            else if (rail == dir.middle)
            {
                rail = dir.left;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(5, 0, -2)));

            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isDashing && !isSliding && isGrounded)
        {
            if (rail == dir.left)
            {
                rail = dir.middle;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + Quaternion.FromToRotation(new Vector3(0, 0, 1), dashDirection) * new Vector3(-5, 0, -2)));
            }
            else if (rail == dir.middle)
            {
                rail = dir.right;
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
