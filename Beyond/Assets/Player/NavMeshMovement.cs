using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left = 0,
    Middle = 1,
    Right = 2    
}

public class NavMeshMovement : MonoBehaviour
{
    public Camera cam;
    
    public UnityEngine.AI.NavMeshAgent agent;
    
    [SerializeField] public Transform playerTransform;
    [SerializeField] public List<Transform> leftPoints;
    [SerializeField] public List<Transform> middlePoints;
    [SerializeField] public List<Transform> rightPoints;

    [SerializeField] public float dashTime = 0.2f;
    public int pointDistance = 8;
    private bool isDashing = false;
    private int waypointIndex = 0;
    private Direction rail;
    private List<List<Transform>> rails;

    void Start()
    {
        rail = Direction.Middle;
        rails = new List<List<Transform>>() { leftPoints, middlePoints, rightPoints};
    }


    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        agent.SetDestination(hit.point);
        //    }
        //}

        //Debug.Log(Vector3.Distance(middlePoints[0].position, playerTransform.position));

        //if (Vector3.Distance(middlePoints[0].position, playerTransform.position) < 1.5 && middlePoints.Count > 1)
        //{
        //    index++;
        //}

        
        if (!isDashing) // Perform dash if not already dashing
        {
            if (Input.GetKeyDown(KeyCode.A)) // Dash left
            {
                if (rail == Direction.Right)
                {
                    rail = Direction.Middle;
                    StartCoroutine(startDashCooldown());
                    StartCoroutine(dashToPosition(transform.position + new Vector3(5, 0, -1)));
                }
                else if (rail == Direction.Middle)
                {
                    rail = Direction.Left;
                    StartCoroutine(startDashCooldown());
                    StartCoroutine(dashToPosition(transform.position + new Vector3(5, 0, -1)));
                }
            }
            else if (Input.GetKeyDown(KeyCode.D)) // Dash right
            {
                if (rail == Direction.Left)
                {
                    rail = Direction.Middle;
                    StartCoroutine(startDashCooldown());
                    StartCoroutine(dashToPosition(transform.position + new Vector3(-5, 0, -1)));
                }
                else if (rail == Direction.Middle)
                {
                    rail = Direction.Right;
                    StartCoroutine(startDashCooldown());
                    StartCoroutine(dashToPosition(transform.position + new Vector3(-5, 0, -1)));
                }
            }
        }

        // Use enum ID to obtain current list of waypoints
        List<Transform> currentRail = rails[(int) rail];
        if (Vector3.Distance(currentRail[waypointIndex].position, playerTransform.position) < pointDistance)
        {
            waypointIndex++;
        }
        agent.SetDestination(currentRail[waypointIndex].position);
    }
    
    IEnumerator dashToPosition(Vector3 finalPos)
    {
        Debug.Log(finalPos);
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

    IEnumerator startDashCooldown()
    {
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
}
