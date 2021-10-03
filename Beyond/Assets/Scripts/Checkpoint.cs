using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private bool isFinishLine = false;
    private Vector3 spawnPos;

    void Start()
    {
        spawnPos = transform.Find("Spawn Point").position;
    }

    public Vector3 GetSpawnPos()
    {
        return spawnPos;
    }

    public bool IsFinishLine()
    {
        return isFinishLine;
    }
}
