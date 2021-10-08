using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum dir
//{
//    left,
//    right,
//    middle
//}

public class LerpMovement : MonoBehaviour
{
    //All points on all three paths: left, middle, right
    public List<Transform> leftPoints = new List<Transform>();
    public List<Transform> middlePoints = new List<Transform>();
    public List<Transform> rightPoints = new List<Transform>();

    public bool isDashing = false;
    public float dashTime = 0.2f;
    public float speed = 10.0f;

    //At which point it is at in its pathing
    public int index = 0;
    public dir rail = dir.middle;

    public bool isGrounded = true;
    public bool isFalling;

    public float hangFactor = 1.5f;
    public float jumpVel = 15f;
    public float counterJumpForce = -5f;

    private Rigidbody rb;

    public bool jumpKeyHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
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

            if (rail == dir.right)
            {
                rail = dir.middle;
                //transform.Translate(-5, 0, 0);
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(5, 0, -2)));
            }
            else if (rail == dir.middle)
            {
                rail = dir.left;
                //transform.Translate(-5, 0, 0);
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(5, 0, -2)));
                //Debug.Log("DASH");

            }
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isDashing)
        {
            if (rail == dir.left)
            {
                rail = dir.middle;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(-5, 0, -2)));
            }
            else if (rail == dir.middle)
            {
                rail = dir.right;
                StartCoroutine(dashWait());
                StartCoroutine(dash(transform.position + new Vector3(-5, 0, -2)));
            }
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

        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move

        Vector3 feetPos = transform.position;
        feetPos.y -= 0.25f;
        if (Physics.Raycast(feetPos, transform.forward * -1, out RaycastHit fwdHit) && fwdHit.distance <= 1)
        {
            Debug.Log("don't move");
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
