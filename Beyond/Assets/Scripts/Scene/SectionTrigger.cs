﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.Find("Scene Control").GetComponent<SceneController>().SetNextSection();
            gameObject.SetActive(false);
        }
    }
}
