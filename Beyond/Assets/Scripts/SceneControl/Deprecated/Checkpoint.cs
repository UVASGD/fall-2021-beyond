using UnityEngine;

/**
 * By Eric Weng
 * !Deprecated!
 */
public class Checkpoint : MonoBehaviour
{
    [SerializeField] public readonly bool isFinishLine = false;
    private Vector3 spawnPos;

    void Start()
    {
        spawnPos = transform.Find("Spawn Point").position;
    }

    public Vector3 GetSpawnPos()
    {
        return spawnPos;
    }

}
