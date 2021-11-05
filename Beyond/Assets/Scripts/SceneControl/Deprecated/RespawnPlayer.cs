using UnityEngine;

/**
 * By Eric Weng
 * !Deprecated!
 */
public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject respawnMessage;
    private Vector3 currentSpawnPos;
    private bool waiting; // Wait till players presses button to respawn

    void Start()
    {
        currentSpawnPos = spawnPoint.transform.position;
        respawnMessage.SetActive(false);
        waiting = false;
    }

    void Update()
    {
        // If fall out of world respawn
        if (transform.position.y < -20)
            RespawnGameObject();
        // Wait for player to press spacebar
        if (waiting && Input.GetKeyDown(KeyCode.Space))
            RespawnGameObject();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            currentSpawnPos = checkpoint.GetSpawnPos();
            // Wait to respawn if crossing finish line
            if (!waiting && checkpoint.isFinishLine)
            {
                waiting = true;
                respawnMessage.SetActive(true);
                currentSpawnPos = spawnPoint.transform.position; // set spawn point back to start
                //gameObject.GetComponent<PlayerController>().enabled = false; // disable player movement
            }
        }
    }

    private void RespawnGameObject()
    {
        waiting = false;
        respawnMessage.SetActive(false);
        transform.position = currentSpawnPos;
        //gameObject.GetComponent<PlayerController>().enabled = true;
    }
}
