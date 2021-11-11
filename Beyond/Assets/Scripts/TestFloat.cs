using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFloat : MonoBehaviour
{
    //Moving up or down
    public bool upDir = true;
    public float speed = 3;
    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        target.y += 3;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) <= 0.25f)
        {
            if (upDir)
                target.y -= 3;
            else
                target.y += 3;
            upDir = !upDir;
        }
    }
}
