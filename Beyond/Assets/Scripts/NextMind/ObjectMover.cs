using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 2f;
    public Vector3 moveDistance;
    public bool shouldMove;

    Vector3 startPosition;
    float t = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (shouldMove)
            transform.position = Vector3.Lerp(transform.position, startPosition + moveDistance, moveSpeed * Time.deltaTime);
    }

    public void Move()
    {
        shouldMove = true;
    }

}
