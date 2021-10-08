using UnityEngine;

/**
 * By AJ Nye and Eric Weng
 */
public class PlayerJump : MonoBehaviour
{
    // Jump Parameters
    [SerializeField] private float hangFactor = 1.5f;
    [SerializeField] private float jumpVel = 35f;
    [SerializeField] private float counterJumpForce = -15f;

    // Debug Flags
    private bool isGrounded = true;
    private bool isFalling = false;
    private bool jumpKeyHeld = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.y < 0)
        {
            isFalling = true;
        }

        // Set initial jump force
        if (Input.GetKeyDown(KeyCode.Space) && isFalling)
        {
            jumpKeyHeld = true;
            // Recheck if standing on ground
            if (!isGrounded)
            {
                if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hit))
                {
                    if (hit.distance <= (GetComponent<Collider>().bounds.size.y / 2) + hangFactor)
                    {
                        isGrounded = true;
                    }
                }
            }
            // Jump if on ground
            if (isGrounded)
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

        // Add counter jump force to make a "smaller" jump when space not held
        if (!isGrounded && !jumpKeyHeld)
        {
            rb.AddForce(counterJumpForce * transform.up * rb.mass);
        }
    }
}
