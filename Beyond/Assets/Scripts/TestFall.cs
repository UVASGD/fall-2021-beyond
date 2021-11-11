using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFall : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        StartCoroutine(fall());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator fall()
    {
        yield return new WaitForSeconds(6f);
        rb.isKinematic = false;
    }
}
