using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpin : MonoBehaviour
{
    
    public float rotSpeed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, rotSpeed, 0);
    }
}
