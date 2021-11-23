using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speedH = 2.0f;
    [SerializeField] private float speedV = 2.0f;

    [SerializeField] private float yaw = 0.0f; // can set starting rotation
    private float pitch = 0.0f;

    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        // Clamp pitch so we don't get weird angles
        if (pitch < -90f) pitch = -90f;
        else if (pitch > 90f) pitch = 90f;

        transform.eulerAngles = new Vector3(pitch, yaw + 180, 0.0f);
    }
}
