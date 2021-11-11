using UnityEngine;

public class DebugCamera : MonoBehaviour
{
    [SerializeField] private float speedH = 2.0f;
    [SerializeField] private float speedV = 2.0f;

    [SerializeField] private float startingYaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        startingYaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, startingYaw, 0.0f);
    }
}
