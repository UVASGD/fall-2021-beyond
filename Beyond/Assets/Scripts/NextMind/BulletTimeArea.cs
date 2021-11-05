using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimeArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Time.timeScale = 0.4f;
            other.GetComponent<NextMindPlayerMovement>().SetForwardForce(50f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Time.timeScale = 1f;
            other.GetComponent<NextMindPlayerMovement>().SetForwardForce(250f);
        }
    }
}
