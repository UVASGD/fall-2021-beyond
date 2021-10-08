using UnityEngine;

/**
 * By Eric Weng
 */
public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;

    private void Update()
    {
        transform.position = target.transform.position + offset;
    }
}
