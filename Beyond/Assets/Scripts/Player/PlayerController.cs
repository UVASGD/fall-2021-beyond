using UnityEngine;

/**
 * By Eric Weng
 */
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
        //rb.AddForce(input, ForceMode.VelocityChange);

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(speed * Vector3.up, ForceMode.Impulse);
            canJump = false;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            canJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OverlayController.SetTextBoxActive(other.name, true);
    }

    private void OnTriggerExit(Collider other)
    {
        OverlayController.SetTextBoxActive(other.name, false);
    }
}
