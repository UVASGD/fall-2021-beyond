using UnityEngine;

/**
 * By AJ Nye
 * Currently unused
 */
public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float vel = 20f;
    public float jumpVel = 15f;
    public bool isGrounded;
    public bool isFalling;
    public float hangFactor = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isGrounded)
        {
            if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit))
            {
                Debug.Log(hit.distance);
                if (rb.velocity.y <= 0)
                {
                    isFalling = true;
                }
                if (hit.distance <= (GetComponent<Collider>().bounds.size.y / 2)*hangFactor)
                {
                    isGrounded = true;
                }
            }
        }
        //rb.AddForce(transform.forward * vel, ForceMode.Force);
        if (Input.GetKey(KeyCode.W)) {
            rb.AddForce(transform.forward * vel, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.A)) {
            rb.AddForce(transform.right * -1 * vel, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.S)) {
            rb.AddForce(transform.forward * -1 * vel, ForceMode.Force);
        }
        if (Input.GetKey(KeyCode.D)) {
            rb.AddForce(transform.right * vel, ForceMode.Force);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded && isFalling)
            {
                isGrounded = false;

                Vector3 vel = rb.velocity;
                vel.y = 0;
                rb.velocity = vel;

                rb.AddForce(transform.up * jumpVel, ForceMode.Impulse);
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit)) {
    //        Debug.Log(hit.distance);
    //        if (hit.distance <= GetComponent<Collider>().bounds.size.y/2)
    //        {
    //            isGrounded = true;
    //            //isFalling = false;
    //        }
    //    }
    //}
}
