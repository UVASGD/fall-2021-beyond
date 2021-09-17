using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;
    private Vector3 spawnPos;

    void Start()
    {
        spawnPos = spawnPoint.transform.position;
    }

    void Update()
    {
        if (transform.position.y < -20)
            RespawnGameObject();
    }

    private void RespawnGameObject()
    {
        transform.position = spawnPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
            RespawnGameObject();
    }
}
