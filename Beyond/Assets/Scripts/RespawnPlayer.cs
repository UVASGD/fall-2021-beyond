using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject respawnMessage;
    private Vector3 spawnPos;
    private bool waiting; // Wait till the playe pressed a button to respawn

    void Start()
    {
        spawnPos = spawnPoint.transform.position;
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
        if (!waiting && other.CompareTag("Respawn")) {
            waiting = true;
            respawnMessage.SetActive(true);
            gameObject.GetComponent<PlayerController>().enabled = false; // disable player movement
        }
    }

    private void RespawnGameObject()
    {
        waiting = false;
        respawnMessage.SetActive(false);
        transform.position = spawnPos;
        gameObject.GetComponent<PlayerController>().enabled = true;
    }
}
