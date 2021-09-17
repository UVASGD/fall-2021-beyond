using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    private bool canJump = true;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 frontBackInput = Vector3.forward * Input.GetAxis("Vertical");
        Vector3 leftRight = Vector3.right * Input.GetAxis("Horizontal");
        Vector3 input = speed * Time.deltaTime * (frontBackInput + leftRight);
        transform.Translate(input);
        //rb.AddForce(input, ForceMode.Force);

        if (canJump && Input.GetKeyDown(KeyCode.Space))
            rb.AddForce(10 * Vector3.up, ForceMode.Impulse);
    }
}
