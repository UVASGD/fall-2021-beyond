using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestructor : MonoBehaviour
{
    public void DestroyObject()
    {
        this.gameObject.SetActive(false);
    }
}
